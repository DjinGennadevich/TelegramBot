using System.IO;
using System.Net;

namespace TelegramBot
{
    public static class YandexApi
    {
        public const string DUMP_FILE_NAME = "organizations.json";

        private static readonly string UrlTemplate;

        static YandexApi()
        {
            UrlTemplate = $"https://search-maps.yandex.ru/v1/?text={{1}},{{0}}&type=biz&lang=ru_RU&results=3&apikey={Configuration.YandexApiKey}";
        }

        public static void SearchByOrganizationAndDumpToFile(string city, string organization)
        {
            string organizationInfoStr = SearchOrganization(city, organization);
            File.WriteAllText(DUMP_FILE_NAME, organizationInfoStr);
        }

        private static string SearchOrganization(string city, string organization)
        {
            string url = string.Format(UrlTemplate, city, organization);

            var request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            using (var data = response.GetResponseStream())
            using (var reader = new StreamReader(data))
                return reader.ReadToEnd();
        }

        public static RootObject SearchByOrganization(string city, string organization)
        {
            string url = string.Format(UrlTemplate, city, organization);

            var request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
                return OrganizationInfoDeserializer.Deserialize(stream);
        }
    }
}
