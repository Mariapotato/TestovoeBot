using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using CRMPark.Commands.Interfaces;
using NLog;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using CRMParkBot.Models;

namespace CRMParkBot.Commands
{
    public class GetInnCommand : ICommand
    {
        private const string Command = "/inn";
        private readonly TelegramBotClient _botClient;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _dadataApiKey;

        public GetInnCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _dadataApiKey = configuration["Settings:DadataApiKey"];
        }

        public async Task<bool> Contains(Update update)
        {
            if (!(update.Type == UpdateType.Message && update.Message?.Text?.StartsWith(Command) == true))
            {
                return false;
            }

            var chatId = update.Message.Chat.Id;
            var innList = update.Message.Text.Replace(Command, "").Split(',').Select(inn => inn.Trim()).ToList();

            if (innList.Any())
            {
                var companyInfoList = await GetCompanyInfoFromDadata(innList);

                foreach (var companyInfo in companyInfoList)
                {
                    var response = $"ИНН: {companyInfo.Inn}\nНазвание: {companyInfo.Name}\nАдреса:\n{string.Join("\n", companyInfo.Addresses)}";
                    await _botClient.SendTextMessageAsync(chatId, response);
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, "Пожалуйста, укажите хотя бы один ИНН.");
            }

            return true;
        }

        public async Task ExecuteAsync(Update update)
        {
            // Логика выполнения команды, если необходимо
        }

        private async Task<List<CompanyInfo>> GetCompanyInfoFromDadata(List<string> innList)
        {
            var client = new RestClient("https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party");
            var companyInfoList = new List<CompanyInfo>();

            foreach (var inn in innList)
            {
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", $"Token {_dadataApiKey}");
                request.AddJsonBody(new { query = inn });

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var content = response.Content;
                    var json = JObject.Parse(content);
                    var suggestions = json["suggestions"].Children().ToList();

                    if (suggestions.Any())
                    {
                        var addresses = new List<string>();
                        var name = suggestions[0]["value"]?.ToString();

                        foreach (var suggestion in suggestions)
                        {
                            addresses.Add(suggestion["data"]["address"]["value"]?.ToString());
                        }

                        var company = new CompanyInfo
                        {
                            Inn = inn,
                            Name = name,
                            Addresses = addresses
                        };
                        companyInfoList.Add(company);
                    }
                    else
                    {
                        companyInfoList.Add(new CompanyInfo { Inn = inn, Name = "ИНН не найден", Addresses = new List<string> { "Адрес не найден" } });
                    }
                }
                else
                {
                    companyInfoList.Add(new CompanyInfo { Inn = inn, Name = "ИНН не найден", Addresses = new List<string> { "Адрес не найден" } });
                }
            }

            return companyInfoList;
        }
    }

    public class CompanyInfo
    {
        public string Inn { get; set; }
        public string Name { get; set; }
        public List<string> Addresses { get; set; }
    }
}
