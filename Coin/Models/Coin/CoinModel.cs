using Coin.DataService;
using Coin.Models.Base;
using Coin.Models.Coin;
using System.Data;
using System.Data.Common;

namespace Coin.Models
{
	public class CoinModel : DataAccessBaseModel
	{
		#region List

		public List<CoinPo> GetListData(CoinVo vo)
		{
			string sql = $@"
                    SELECT          ROW_NUMBER() OVER (ORDER BY CM.CoinMainId) AS RowIndex,
									CM.CMCode, CMRate, CMRateFloat, CMUpdateTime,
									CoinMapNameId, CMNName, CMNUpdateTime
                    FROM            {_SchemaName}.CoinMain CM
					LEFT	JOIN	{_SchemaName}.CoinMapName CMN ON CM.CMCode = CMN.CMCode
                    WHERE           1=1 
					";
			if (!string.IsNullOrEmpty(vo.CMCode))
			{
				sql += " AND CM.CMCode = @CMCode ";
			}
			if (!string.IsNullOrEmpty(vo.CMNName))
			{
				sql += " AND CMN.CMNName LIKE @CMNName ";
			}
			string[] fNames = new string[] { "CMCode", "CMNName" };
			string[] fValues = new string[] { vo.CMCode, "%" + vo.CMNName + "%" };
			List<CoinPo> poList = new List<CoinPo>();
			using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
			{
				DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
				using (DbDataReader reader = DataSetHelper.ExecuteReader(dbConn, CommandType.Text, sql, dbParams))
				{
					while (reader.Read())
					{
						CoinPo po = new CoinPo
						{
							RowIndex = Convert.ToString(reader["RowIndex"]) ?? "".Trim(),
							CMCode = Convert.ToString(reader["CMCode"]) ?? "".Trim(),
							CMRate = Convert.ToString(reader["CMRate"]) ?? "".Trim(),
							CMRateFloat = Convert.ToDecimal(reader["CMRateFloat"]),
							CMUpdateTime = Convert.ToString(reader["CMUpdateTime"] ?? "".Trim()),
							CoinMapNameId = Convert.ToString(reader["CoinMapNameId"] ?? "".Trim()),
							CMNName = Convert.ToString(reader["CMNName"]) ?? "".Trim(),
							CMNUpdateTime = Convert.ToString(reader["CMNUpdateTime"] ?? "".Trim()),
						};
						poList.Add(po);
					}
				}
			}
			return poList;
		}

		public int GetListCount(string CMCode)
		{
			string sql = $@"
                SELECT		COUNT(*) COUNT
                FROM        {_SchemaName}.CoinMapName CMN
                WHERE       CMCode = @CMCode  ";

			string[] fNames = new string[] { "CMCode" };
			string[] fValues = new string[] { CMCode };
			int count = 0;
			using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
			{
				DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
				using (DbDataReader reader = DataSetHelper.ExecuteReader(dbConn, CommandType.Text, sql, dbParams))
				{
					while (reader.Read())
					{
						count = Convert.ToInt32(reader["COUNT"]);
					}
				}
			}
			return count;
		}
		#endregion

		#region <Form>

		public CoinPo? GetFormData(string CoinMapNameId)
		{
			string sql = $@"
                SELECT		CMCode, CMNName
                FROM        {_SchemaName}.CoinMapName CMN
                WHERE       CoinMapNameId = @CoinMapNameId  ";

			string[] fNames = new string[]  { "CoinMapNameId" };
			string[] fValues = new string[]  { CoinMapNameId };
			CoinPo po = null;
			using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
			{
				DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
				using (DbDataReader reader = DataSetHelper.ExecuteReader(dbConn, CommandType.Text, sql, dbParams))
				{
					while (reader.Read())
					{
						po = new CoinPo
						{
							CMCode = Convert.ToString(reader["CMCode"]) ?? "".Trim(),
							CMNName = Convert.ToString(reader["CMNName"]) ?? "".Trim(),
						};
					}
				}
			}
			return po;
		}

		#endregion

		#region Action

		public DbExecuteResult ActionInsertData(CoinPo po)
		{
			if (po == null)
			{
				throw new ArgumentNullException(nameof(po));
			}
			string sql = $@"
                INSERT INTO {_SchemaName}.CoinMain(
                            CMCode, CMRate, CMRateFloat, CMCreateTime) 
                VALUES(     @CMCode, @CMRate, @CMRateFloat, @CMCreateTime) ";

			string[] fNames = new string[] {
							"CMCode", "CMRate", "CMRateFloat", "CMCreateTime" };
			string[] fValues = new string[] {
							 po.CMCode, po.CMRate, po.CMRateFloat.ToString(), po.CMCreateTime};
			try
			{
				using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
				{
					DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
					return DataSetHelper.ExecuteNonQuery(dbConn, CommandType.Text, sql, dbParams) > 0
							? DbExecuteResult.Success : DbExecuteResult.NoDataInsert;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return DbExecuteResult.Exception;
			}
		}

		public DbExecuteResult ActionUpdateData(CoinPo po)
		{
			if (po == null)
			{
				throw new ArgumentNullException(nameof(po));
			}
			string updSql = $@"
                UPDATE      {_SchemaName}.CoinMain 
                SET         CMCode = @CMCode, 
							CMRate = @CMRate, 
							CMRateFloat = @CMRateFloat, 
							CMUpdateTime = @CMUpdateTime
                WHERE       CMCode = @CMCode ";

			string[] fNames = new string[] {
							"CMCode", "CMRate", "CMRateFloat", "CMUpdateTime" };
			string[] fValues = new string[] {
							 po.CMCode, po.CMRate, po.CMRateFloat.ToString(), po.CMUpdateTime };
			try
			{
				using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
				{
					DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
					return DataSetHelper.ExecuteNonQuery(dbConn, CommandType.Text, updSql, dbParams) > 0
							? DbExecuteResult.Success : DbExecuteResult.NoDataUpdate;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return DbExecuteResult.Exception;
			}
		}

		public DbExecuteResult ActionInsertMapData(CoinPo po)
		{
			if (po == null)
			{
				throw new ArgumentNullException(nameof(po));
			}
			string sql = $@"
                INSERT INTO {_SchemaName}.CoinMapName(
                            CMCode, CMNName, CMNCreateTime) 
                VALUES(     @CMCode, @CMNName, GETDATE()) ";

			string[] fNames = new string[] {
							"CMCode", "CMNName" };
			string[] fValues = new string[] {
							 po.CMCode, po.CMNName};
			try
			{
				using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
				{
					DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
					return DataSetHelper.ExecuteNonQuery(dbConn, CommandType.Text, sql, dbParams) > 0
							? DbExecuteResult.Success : DbExecuteResult.NoDataInsert;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return DbExecuteResult.Exception;
			}
		}

		public DbExecuteResult ActionUpdateMapData(CoinPo po)
		{
			if (po == null)
			{
				throw new ArgumentNullException(nameof(po));
			}
			string updSql = $@"
                UPDATE      {_SchemaName}.CoinMapName 
                SET         CMNName = @CMNName,
							CMNUpdateTime = GETDATE()
                WHERE       CoinMapNameId = @CoinMapNameId
						AND	CMCode = @CMCode ";

			string[] fNames = new string[] {
							"CoinMapNameId", "CMCode", "CMNName" };
			string[] fValues = new string[] {
							 po.CoinMapNameId, po.CMCode, po.CMNName };
			try
			{
				using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
				{
					DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
					return DataSetHelper.ExecuteNonQuery(dbConn, CommandType.Text, updSql, dbParams) > 0
							? DbExecuteResult.Success : DbExecuteResult.NoDataUpdate;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return DbExecuteResult.Exception;
			}
		}

		public DbExecuteResult ActionDeleteMapData(CoinPo po)
		{
			if (po == null)
			{
				throw new ArgumentNullException(nameof(po));
			}

			string delSql = $@"
                DELETE
                FROM        {_SchemaName}.CoinMapName 
                 WHERE       CoinMapNameId = @CoinMapNameId AND CMCode = @CMCode ";

			string[] fNames = new string[] { "CoinMapNameId", "CMCode" };
			string[] fValues = new string[] { po.CoinMapNameId, po.CMCode };
			try
			{
				using (DbConnection dbConn = DataSetService.GetConnection(_DatabaseId))
				{
					DbParameter[] dbParams = DataSetService.GetTableParams(fNames, fValues);
					return DataSetHelper.ExecuteNonQuery(dbConn, CommandType.Text, delSql, dbParams) > 0
							? DbExecuteResult.Success : DbExecuteResult.NoDataDelete;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return DbExecuteResult.Exception;
			}
		}
		#endregion
	}
}
