using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Net.Mail;
using CRMPark.Commands.Interfaces;
using NLog;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using RestSharp;

namespace CRMParkBot.Commands
{
    public class GetAllCommand : ICommand
    {
        private const string Command = "/help";
        private readonly TelegramBotClient _botClient;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public GetAllCommand(TelegramBotClient botClient)
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
            await _botClient.SendTextMessageAsync(chatId, "Доступные команды: /start, /help, /hello, /inn, /last", replyMarkup: new ForceReplyMarkup { Selective = true }); ;

            return false;
        }

        public async Task ExecuteAsync(Update update)
        {
            
        }

      
        
    }
}
