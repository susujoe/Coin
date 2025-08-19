using Coin.Controllers;
using Coin.Handling;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ���U HttpClient
builder.Services.AddHttpClient<CoinController>();

// �[�J Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// �إ� app
var app = builder.Build();

// Middleware (�ҥ~�B�z)
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // ��ܧ��� Stack Trace
}
else
{
	app.UseExceptionHandler("/Coin/Error"); // �ɦV��ۭq���~��
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
	c.RoutePrefix = string.Empty; // Swagger UI �}�b���� (�i��)
});

// �]�w����
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Coin}/{action=Index}/{id?}");

app.Run();