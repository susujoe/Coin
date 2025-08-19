namespace Coin.Helper
{
	public class ConfigurationHelper
	{
		public static IConfigurationRoot Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		private static string GetKeyString(string key)
		{
			return $"SiteSettings:{key}";
		}

		public static string GetValue(string key)
		{
			return Configuration.GetValue<string>(GetKeyString(key)) ?? "";
		}

		public static string GetString(string key)
		{
			return Configuration.GetValue<string>(GetKeyString(key)) ?? "";
		}

		public static bool GetBoolean(string key)
		{
			if (bool.TryParse(Configuration.GetValue<string>(GetKeyString(key)) ?? "", out bool result))
			{
				return result;
			}
			return false;
		}

		public static bool GetInteger(string key, out int result)
		{
			return Int32.TryParse(Configuration.GetValue<string>(GetKeyString(key)) ?? "", out result);
		}

		public static int GetInteger(string key)
		{
			Int32.TryParse(Configuration.GetValue<string>(GetKeyString(key)) ?? "", out int result);
			return result;
		}

		public static IConfiguration GetSection(string key)
		{
			return Configuration.GetSection(GetKeyString(key));
		}

		public static string GetConnString(string databaseId)
		{
			return Configuration.GetConnectionString(databaseId) ?? string.Empty;
		}
	}
}
