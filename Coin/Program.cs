using Coin.Controllers;
using Coin.Handling;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 註冊 HttpClient
builder.Services.AddHttpClient<CoinController>();

// 加入 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// 建立 app
var app = builder.Build();

// Middleware (例外處理)
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // 顯示完整 Stack Trace
}
else
{
	app.UseExceptionHandler("/Coin/Error"); // 導向到自訂錯誤頁
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
	c.RoutePrefix = string.Empty; // Swagger UI 開在首頁 (可選)
});

// 設定路由
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Coin}/{action=Index}/{id?}");

app.Run();