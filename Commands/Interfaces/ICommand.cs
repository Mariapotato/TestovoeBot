using System.Threading.Tasks;

using Telegram.Bot.Types;

namespace CRMPark.Commands.Interfaces
{
    public interface ICommand
    {
        Task<bool> Contains(Update update);

        Task ExecuteAsync(Update update);
    }
}