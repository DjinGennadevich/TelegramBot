namespace TelegramBot
{
    internal class BotMessages
    {
        public const string START_MESSAGE = "Для вывода меню напишите команду /help, либо нажмите на нее.";
        
        public const string HELP_MESSAGE = @"/start - Запуск бота.
/help - Отображение списка команд.
/showInformation - Вывод добавленной информации. 
/institutions - Поиск общепитов. 
/attractions - Узнать информацию о достопримечательностях.
/news - Свежие новости.
Для добавления данных об городе, просто отправьте сообщение боту на русском языке, без выбора конкретного пункта меню, например (Город-Минск).";
        
        public const string CITY_ADDED_MESSAGE = "Добавление данных произошло успешно, ознакомиться с ними можно во вкладке /showInformation.";

        public const string ATTRACTIONS = "Музеи";
        public const string INSTITUTIONS = "Рестораны";
        public const string NEWS = "Новости";
               
        private const string MESSAGE_WITH_CITY_TEMPLATE = "В данный момент у вас выбран город [{0}].";
        public static string GetCityMessage(string cityName)
        {
            return string.Format(MESSAGE_WITH_CITY_TEMPLATE, cityName);
        }

    }
}
