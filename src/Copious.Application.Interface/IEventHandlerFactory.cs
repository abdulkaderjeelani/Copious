using System.Collections.Generic;
using Copious.Foundation;

namespace Copious.Application.Interface
{
    public interface IEventHandlerFactory
    {
        IEnumerable<IEventHandler<T>> GetHandlers<T>() where T : Event;

        IEnumerable<IEventHandlerAsync<T>> GetAsyncHandlers<T>() where T : Event;
    }
}