namespace Coin.Models.Coin
{
	public class BasePo
	{
		public bool IsSuccess { get; set; } = false;
		public string Code { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public object Data { get; set; } = new object();
	}
}
