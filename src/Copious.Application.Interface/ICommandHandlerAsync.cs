using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Application.Interface
{
    public interface ICommandHandlerAsync<in TCommand> : ICommandHandler where TCommand : Command
    {
        Task ExecuteAsync(TCommand command);
    }
}