using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using CRMPark.Commands.Interfaces;
using NLog;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace CRMParkBot.Commands
{
    public class LastCommand : ICommand
    {
        private const string Command = "/last";
        private readonly TelegramBotClient _botClient;
        private readonly Dictionary<long, string> _lastCommands;
        private readonly List<ICommand> _commands;

        public LastCommand(TelegramBotClient botClient, Dictionary<long, string> lastCommands)
        {
            _botClient = botClient;
            _lastCommands = lastCommands;
        }

        public async Task<bool> Contains(Update update)
        {
            if (!(update.Type == UpdateType.Message && update.Message?.Text == Command))
            {
                return false;
            }

            var chatId = update.Message.Chat.Id;

            return true;
        }

        public async Task ExecuteAsync(Update update)
        {
            // Логика выполнения команды, если необходимо
        }
    }
}
