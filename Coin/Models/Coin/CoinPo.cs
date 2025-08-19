namespace Coin.Models.Coin
{
	public class CoinPo
	{
		public string RowIndex { get; set; } = string.Empty;
		public string CMCode { get; set; } = string.Empty;
		public string CMSymbol { get; set; } = string.Empty;
		public string CMRate { get; set; } = string.Empty;
		public decimal CMRateFloat { get; set; } = 0;
		public string CMDescription { get; set; } = string.Empty;
		public string RateId { get; set; } = string.Empty;
		public string CMUpdateTime { get; set; } = string.Empty;
		public string CMCreateTime { get; set; } = string.Empty;
		public string CoinMapNameId { get; set; } = string.Empty;
		public string CMNName { get; set; } = string.Empty;
		public string CMNUpdateTime { get; set; } = string.Empty;
	}
}
