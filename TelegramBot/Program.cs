using System;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var telegramBot = new TelegramBotEditor(Configuration.TelegramBotToken, Configuration.ClientAccessToken))
            {
                telegramBot.StartListening();

                Console.WriteLine("Bot is started. Press any key to stop the bot...");
                Console.ReadKey();

                telegramBot.StopListening();
                
                Console.WriteLine("Bot is stopped.");
            }

            Console.ReadLine();
        }
    }
}
