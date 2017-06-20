using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Application.Interface
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : Event
    {
        Task HandleAsync(TEvent @event);
    }
}