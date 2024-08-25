using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CRMParkBot.Models;
using NLog;

namespace CRMParkBot.Core
{
    public static class BotServiceExtensions
    {
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public static async Task<IMvcBuilder> AddTelegramBotAsync(this IMvcBuilder builder, IConfiguration configuration)
        {

            try
            {
                var appSettings = new Settings();
                configuration.GetSection(nameof(Settings)).Bind(appSettings);


                var bot = new Bot(appSettings.Token, appSettings.ApplicationUrl);


                await bot.GetBotClientAsync();


                builder.Services.AddSingleton(bot);

                _logger.Info("Telegram bot added to service collection.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error configuring Telegram bot.");
                throw;
            }

            return builder;
        }
    }
}
