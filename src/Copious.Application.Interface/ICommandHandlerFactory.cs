using System.Collections.Generic;
using Copious.Foundation;

namespace Copious.Application.Interface
{
    public interface ICommandHandlerFactory
    {
        IEnumerable<ICommandHandler<T>> GetHandlers<T>() where T : Command;

        IEnumerable<ICommandHandlerAsync<T>> GetAsyncHandlers<T>() where T : Command;
    }
}