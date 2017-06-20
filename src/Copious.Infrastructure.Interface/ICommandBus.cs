using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface
{
    public interface ICommandBus
    {
        void Send<T>(Envelope<T> command) where T : Command;

        Task SendAsAsync<T>(Envelope<T> command) where T : Command;

        Task SendAsync<T>(Envelope<T> command) where T : Command;

        void SendAsyncAsSync<T>(Envelope<T> command) where T : Command;
    }
}