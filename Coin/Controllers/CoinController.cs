using Coin.Models;
using Coin.Models.Coin;
using Coin.ViewModels.Coin;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static Coin.Models.Base.DataAccessBaseModel;

namespace Coin.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class CoinController : ControllerBase
	{
		private readonly HttpClient _httpClient;

		public CoinController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		[HttpGet("GetRates")]
		public IActionResult GetRates()
		{
			//string apiUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
			try
			{

				bool success = false;
				string message = string.Empty;
				//HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

				//response.EnsureSuccessStatusCode();

				//string result = await response.Content.ReadAsStringAsync();

				//return Ok(new
				//{
				//	success = true,
				//	data = result
				//});

				var jsonContent = System.IO.File.ReadAllText("data.json");
				var root = JsonNode.Parse(jsonContent);

				var updatedISO = root?["time"]?["updatedISO"]?.ToString();

				var bpi = root?["bpi"]?.AsObject();
				if (bpi != null)
				{
					CoinVo coinVo = new CoinVo();
					CoinModel coinModel = new CoinModel();
					List<CoinPo> poList = coinModel.GetListData(coinVo);
					foreach (var val in bpi)
					{
						string code = val.Key;
						var item = val.Value.AsObject();
						string symbol = item["symbol"]?.ToString() ?? "";
						string rate = item["rate"]?.ToString() ?? "";
						decimal rateFloat = decimal.Parse(item["rate_float"]?.ToString() ?? "0");
						

						CoinPo coinPo = coinPo = new CoinPo
						{
							CMCode = code,
							CMSymbol = symbol,
							CMRate = rate,
							CMRateFloat = rateFloat,
							CMDescription = item["description"]?.ToString() ?? "",
							CMUpdateTime = DateTime.Parse(updatedISO).ToString("yyyy/MM/dd HH:mm:ss"),
							CMCreateTime = DateTime.Parse(updatedISO).ToString("yyyy/MM/dd HH:mm:ss"),
						};

						try
						{
							int count = poList.Count(po => po.CMCode == coinPo.CMCode);
							if (count > 0)
							{
								DbExecuteResult result = coinModel.ActionUpdateData(coinPo);
								if (result == DbExecuteResult.Success)
								{
									success = true;
									message = $"更新成功";
								}
								else
								{
									message = $"更新失敗，錯誤代碼：{result}";
								}
							}
							else
							{
								DbExecuteResult result = coinModel.ActionInsertData(coinPo);
								if (result == DbExecuteResult.Success)
								{
									success = true;
									message = $"新增成功";
								}
								else
								{
									message = $"新增失敗，錯誤代碼：{result}";
								}
							}
						}
						catch (Exception ex)
						{
							return Ok(new
							{
								success = false,
								message = "更新錯誤：" + ex.Message
							});
						}
					}
				}
				return Ok(new
				{
					success = success,
					message = message
				});
			}
			catch (HttpRequestException ex)
			{
				return StatusCode(500, new
				{
					success = false,
					message = $"HTTP 錯誤：{ex.Message}"
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new
				{
					success = false,
					message = $"發生錯誤：{ex.Message}"
				});
			}
		}
		[HttpGet("GetList")]
		public IActionResult GetList([FromQuery] CoinViewModel model)
		{
			string outputJson = string.Empty;
			BasePo baseJson = new BasePo();
			try
			{
				CoinVo coinVo = new CoinVo
				{
					CMCode = model.CMCode,
					CMNName = model.CMNName
				};

				CoinModel coinModel = new CoinModel();
				List<CoinPo> poList = coinModel.GetListData(coinVo);
				if(poList != null)
				{
					baseJson = new BasePo
					{
						IsSuccess = true,
						Message = "取得資料成功",
						Code = "200",
						Data = poList
					};
					outputJson = JsonSerializer.Serialize(baseJson);
				}
			}
			catch (Exception ex)
			{
				baseJson = new BasePo
				{
					IsSuccess = true,
					Message = "取得資料失敗：" + ex.Message,
					Code = "404",
				};
			}
			outputJson = JsonSerializer.Serialize(baseJson);
			return Content(outputJson, "application/json", Encoding.UTF8);
		}


		[HttpPost("PostData")]
		public IActionResult PostData([FromForm] CoinViewModel model)
		{
			string outputJson = string.Empty;
			BasePo baseJson = new BasePo();
			try
			{
				CoinPo coinPo = new CoinPo
				{
					CMCode = model.CMCode,
					CMNName = model.CMNName,
					CoinMapNameId = model.CoinMapNameId,
				};

				CoinModel coinModel = new CoinModel();
				if (string.Equals("Add", model.FormAction))
				{
					int count = coinModel.GetListCount(coinPo.CMCode);
					if(count > 0)
					{
						baseJson = new BasePo
						{
							IsSuccess = false,
							Message = "資料已存在，請勿重複新增",
							Code = "400",
						};
						outputJson = JsonSerializer.Serialize(baseJson);
						return Content(outputJson, "application/json", Encoding.UTF8);
					}

					DbExecuteResult result = coinModel.ActionInsertMapData(coinPo);
					if (result == DbExecuteResult.Success)
					{
						baseJson = new BasePo
						{
							IsSuccess = true,
							Message = "新增資料成功",
							Code = "200",
						};
						outputJson = JsonSerializer.Serialize(baseJson);
					}
				}
				else if (string.Equals("Edit", model.FormAction))
				{
					DbExecuteResult result = coinModel.ActionUpdateMapData(coinPo);
					if (result == DbExecuteResult.Success)
					{
						baseJson = new BasePo
						{
							IsSuccess = true,
							Message = "更新資料成功",
							Code = "200",
						};
						outputJson = JsonSerializer.Serialize(baseJson);
					}
				}
				else if (string.Equals("Delete", model.FormAction))
				{
					DbExecuteResult result = coinModel.ActionDeleteMapData(coinPo);
					if (result == DbExecuteResult.Success)
					{
						baseJson = new BasePo
						{
							IsSuccess = true,
							Message = "刪除資料成功",
							Code = "200",
						};
						outputJson = JsonSerializer.Serialize(baseJson);
					}
				}
				else
				{
					baseJson = new BasePo
					{
						IsSuccess = true,
						Message = "取得資料失敗" ,
						Code = "404",
					};
				}
					
			}
			catch (Exception ex)
			{
				baseJson = new BasePo
				{
					IsSuccess = true,
					Message = "取得資料失敗：" + ex.Message,
					Code = "404",
				};
			}
			outputJson = JsonSerializer.Serialize(baseJson);
			return Content(outputJson, "application/json", Encoding.UTF8);
		}
	}
}
