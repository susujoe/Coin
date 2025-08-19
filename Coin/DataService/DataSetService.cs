using System.Data.Common;
using Microsoft.Data.SqlClient;
using Coin.Helper;

namespace Coin.DataService
{
    public static class DataSetService
    {
        public static readonly string PARAMSPLITER = "@";

        public static DbConnection GetConnection(string databaseId)
        {
            SqlConnection dbConn = new SqlConnection(ConfigurationHelper.GetConnString(databaseId));
            dbConn.Open();
            return dbConn;
        }

        public static DbParameter[] GetTableParams(string[] FieldNames, string[] FieldValues)
        {
            DbParameter[] sqlParams = new DbParameter[FieldNames.Length];
            for (int i = 0; i < FieldNames.Length; i++)
            {
                sqlParams[i] = new SqlParameter(PARAMSPLITER + FieldNames[i], FieldValues[i]);
            }
            return sqlParams;
        }

        public static DbParameter[] GetTableParams(string[] FieldNames, object[] FieldValues)
        {
            DbParameter[] sqlParams = new DbParameter[FieldNames.Length];
            for (int i = 0; i < FieldNames.Length; i++)
            {
                sqlParams[i] = new SqlParameter(PARAMSPLITER + FieldNames[i], FieldValues[i]);
            }
            return sqlParams;
        }

        public static DbParameter[] GetTableParams(List<string> FieldNames, List<string> FieldValues)
        {
            return GetTableParams(FieldNames.ToArray(), FieldValues.ToArray());
        }

        public static DbParameter[] GetTableParams(List<string> FieldNames, List<object> FieldValues)
        {
            return GetTableParams(FieldNames.ToArray(), FieldValues.ToArray());
        }
    }
}
