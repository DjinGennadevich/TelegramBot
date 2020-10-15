using System.Configuration;

namespace TelegramBot
{
	internal static class Configuration
	{
		private const string TelegramBotTokenKey = "telegramBotToken";
		private const string ClientAccessTokenKey = "clientAccessToken";
		private const string YandexApiKeyKey = "yandexApiKey";

		public static string TelegramBotToken { get; }
		public static string ClientAccessToken { get; }
		public static string YandexApiKey { get; }

		static Configuration()
		{
			TelegramBotToken = ConfigurationManager.AppSettings.Get(TelegramBotTokenKey);
			ClientAccessToken = ConfigurationManager.AppSettings.Get(ClientAccessTokenKey);
			YandexApiKey = ConfigurationManager.AppSettings.Get(YandexApiKeyKey);
		}
	}
}
