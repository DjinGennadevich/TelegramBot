using ApiAiSDK;
using ApiAiSDK.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    internal class TelegramBotEditor : IDisposable
    {
        private bool IsCurrentCitySet => !string.IsNullOrEmpty(_city);

        private readonly string _telegramBotToken;
        private readonly string _clientAccessToken;

        private TelegramBotClient botClient;
        private ApiAi ApiAi;

        private string _city;

        public TelegramBotEditor(string telegramBotToken, string clientAccessToken)
        {
            _telegramBotToken = telegramBotToken;
            _clientAccessToken = clientAccessToken;

            botClient = new TelegramBotClient(_telegramBotToken) { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += BotOnMessageReceived;

            AIConfiguration config = new AIConfiguration(_clientAccessToken, SupportedLanguage.English);
            ApiAi = new ApiAi(config);
        }

        public void StartListening()
        {
            botClient.StartReceiving();
        }

        public void StopListening()
        {
            botClient.StopReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            int senderId = e.Message.From.Id;
            string messageText = e.Message.Text;

            if (MessageType.Text != e.Message.Type)
                return;

            switch (messageText)
            {
                case BotCommands.START:
                    await botClient.SendTextMessageAsync(senderId, BotMessages.START_MESSAGE);
                    break;
                case BotCommands.HELP:
                    await botClient.SendTextMessageAsync(senderId, BotMessages.HELP_MESSAGE);
                    break;
                case BotCommands.SHOW_INFORMATION:
                    await botClient.SendTextMessageAsync(senderId, BotMessages.GetCityMessage(_city));
                    break;
                case BotCommands.INSTITUTIONS:
                    await SendResponseWithOrganizationInfoAsync(senderId, IsCurrentCitySet ? BotMessages.INSTITUTIONS : string.Empty);
                    break;
                case BotCommands.ATTRACTIONS:
                    await SendResponseWithOrganizationInfoAsync(senderId, IsCurrentCitySet ? BotMessages.ATTRACTIONS : string.Empty);
                    break;
                case BotCommands.NEWS:
                    await SendResponseWithOrganizationInfoAsync(senderId, IsCurrentCitySet ? BotMessages.NEWS : string.Empty);
                    break;
                default:
                    if (TrySetCurrentCity(messageText))
                        await botClient.SendTextMessageAsync(senderId, BotMessages.CITY_ADDED_MESSAGE);
                    else
                        await RespondWithAiAnswer(senderId, messageText);
                    break;
            }
        }

        /// <summary>
        /// Этот метод запишет ответ Яндекс API в файл, и тут же будет считан из файла. После этого он отправится пользователю.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationCategory"></param>
        /// <returns></returns>
        private async Task SendResponseWithOrganizationInfoAsync(int userId, string organizationCategory)
        {
            YandexApi.SearchByOrganizationAndDumpToFile(_city, organizationCategory);
            await SendOrganizationInfoResponseAsync(userId);
        }

        /// <summary>
        /// Этот метод читает ответ Яндекс API, десериализует, отправляет пользователю. Нет записи в файл.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationCategory"></param>
        /// <returns></returns>
        private async Task SendResponseWithOrganizationInfoNoDumpAsync(int userId, string organizationCategory)
        {
            var organizationInfo = YandexApi.SearchByOrganization(_city, organizationCategory);
            await SendOrganizationInfoResponseAsync(userId, organizationInfo);
        }

        private async Task SendOrganizationInfoResponseAsync(int userId, RootObject organizationInfo = null)
        {
            organizationInfo = organizationInfo ?? OrganizationInfoDeserializer.Deserialize(YandexApi.DUMP_FILE_NAME);
            foreach (var item in organizationInfo)
            {
                try
                {
                    await botClient.SendTextMessageAsync(userId, $"{item}");
                }
                catch 
                {
                    // TODO: log exception
                }
            }
        }

        private bool TrySetCurrentCity(string cityName)
        {
            _city = Regex.IsMatch(cityName, @"^Город-[а-яА-Я]{3,30}$") ? cityName : null;
            return IsCurrentCitySet;
        }

        private async Task RespondWithAiAnswer(int userId, string question)
        {
            AIResponse response = ApiAi.TextRequest(question);
            string answer = response.Result.Fulfillment.Speech;
            await botClient.SendTextMessageAsync(userId, answer);
        }

        public void Dispose()
        {
            if (botClient != null)
            {
                botClient.OnMessage -= BotOnMessageReceived;
                botClient.StopReceiving();
                botClient = null;
            }
        }
    }
}
