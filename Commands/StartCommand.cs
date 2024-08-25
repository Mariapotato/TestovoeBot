using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Net.Mail;
using CRMPark.Commands.Interfaces;
using NLog;
using Telegram.Bot.Types.ReplyMarkups;

namespace CRMParkBot.Commands
{
    public class StartCommand : ICommand
    {
        private const string Command = "/start";
        private readonly TelegramBotClient _botClient;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public StartCommand(TelegramBotClient botClient)
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
            await _botClient.SendTextMessageAsync(chatId, "Привет! Я бот для получения информации о компаниях по ИНН.", replyMarkup: new ForceReplyMarkup { Selective = true }); ;

            return false;
        }

        public async Task ExecuteAsync(Update update)
        {

        }


    }
}
