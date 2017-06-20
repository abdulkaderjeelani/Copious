using System;
using System.Collections.Generic;

namespace Copious.Persistance.Interface
{
    public interface IQueryHandlerFactory
    {
        IEnumerable<IQueryHandler> GetAsyncHandlers(Type qryType, Type qryResType);

        IEnumerable<IQueryHandler> GetHandlers(Type qryType, Type qryResType);
    }
}