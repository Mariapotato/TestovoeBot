using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using CRMPark.Commands.Interfaces;
using NLog;
using Telegram.Bot.Types.ReplyMarkups;

namespace CRMParkBot.Commands
{
    public class HelloCommand : ICommand
    {
        private const string Command = "/hello";
        private readonly TelegramBotClient _botClient;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public HelloCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task<bool> Contains(Update update)
        {

            if (!(update.Type == UpdateType.Message && update.Message?.Text == Command))
            {
                return false;
            }
            var chatId = update.Message.Chat.Id;
            await _botClient.SendTextMessageAsync(chatId, "Имя и фамилия: Дегтярев Константин\nEmail: kosandeg@gmail.com\nGitHub: https://github.com/Kosan4kk/CRMParkTest", replyMarkup: new ForceReplyMarkup { Selective = true }); ;

            return false;
        }

        public async Task ExecuteAsync(Update update)
        {
        }





    }
}
