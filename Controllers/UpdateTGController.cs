using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using CRMParkBot.Core;
using NLog;
using System.Collections.Generic;
using CRMParkBot.Commands;

namespace CRMParkBot.Controllers
{
    [Route("update")]
    public class UpdateController : Controller
    {
        private readonly Bot _bot;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<long, string> _lastCommands;

        public UpdateController(Bot bot)
        {
            _bot = bot;
            _lastCommands = new Dictionary<long, string>();
        }

        [HttpGet]
        public string Get()
        {
            return "Works!";
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            try
            {
                if (update.Message != null)
                {
                    var chatId = update.Message.Chat.Id;
                    bool commandHandled = false;

                    foreach (var command in _bot.Commands)
                    {
                        if (await command.Contains(update).ConfigureAwait(false))
                        {
                            await command.ExecuteAsync(update).ConfigureAwait(false);
                            commandHandled = true;
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing update from TG");
            }

            return Ok();
        }
    }
}
