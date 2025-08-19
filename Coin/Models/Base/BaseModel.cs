using Coin.Helper;
using System.Data.Common;

namespace Coin.Models.Base
{
	public class DataAccessBaseModel
	{
		protected string _DatabaseId = ConfigurationHelper.GetValue("Database:DatabaseId");
		protected string _SchemaName = ConfigurationHelper.GetValue("Database:SchameName");

		public string Message { protected set; get; } = string.Empty;

		public enum DbExecuteResult
		{
			Success,
			NoDataInsert,
			NoDataUpdate,
			NoDataDelete,
			KeyViolation,
			Exception,
			None
		}

		protected int GetReaderInteger(object value)
		{
			try { return Convert.ToInt32(value); }
			catch { return 0; }
		}

		protected string GetReaderString(object value)
		{
			try { return (Convert.ToString(value) ?? "").Trim(); }
			catch { return string.Empty; }
		}

		protected bool GetReaderBoolean(object value)
		{
			try { return Convert.ToBoolean(value); }
			catch { return false; }
		}

		protected DateTime? GetReaderDateTime(object value)
		{
			try { return Convert.ToDateTime(value); }
			catch { return null; }
		}

		protected string GetReaderDateTimeString(object value)
		{
			try
			{ return (Convert.ToString(value) ?? "").Trim(); }
			catch { return string.Empty; }
		}
	}
}
