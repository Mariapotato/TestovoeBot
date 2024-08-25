using CRMParkBot.Commands;
using CRMPark.Commands.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CRMParkBot.Core
{
    public class Bot
    {
        private readonly string _token;
        private readonly string _applicationUrl;
        private readonly List<ICommand> _commands = new List<ICommand>();
        private TelegramBotClient? _botClient;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
                private readonly Dictionary<long, string> _lastCommands;

        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();

        public Bot(string token, string appUrl)
        {
            _token = token;
            _applicationUrl = appUrl;
        }

        public async Task<ITelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                _logger.Debug("Bot client already exists.");
                return _botClient;
            }

            try
            {
                _botClient = new TelegramBotClient(_token);
                string hook = string.Format(_applicationUrl, "update");
                var allowedUpdated = new List<UpdateType>() { UpdateType.Message, UpdateType.CallbackQuery };
                SetUpCommandList();
                await _botClient.DeleteWebhookAsync();
                await _botClient.GetUpdatesAsync(offset: -1);
                await _botClient.SetWebhookAsync(hook, allowedUpdates: allowedUpdated);

                _logger.Info("Bot client initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error initializing bot client.");
                throw;
            }

            return _botClient;
        }

        private void SetUpCommandList()
        {
            try
            {

                _commands.Add(new StartCommand(_botClient!));
                _commands.Add(new GetAllCommand(_botClient!));
                _commands.Add(new HelloCommand(_botClient!));
                _commands.Add(new GetInnCommand(_botClient!));
                _commands.Add(new LastCommand(_botClient!, _lastCommands));

                _logger.Debug("Commands added successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error setting up command list.");
                throw;
            }
        }

    }
}
