using System;
using System.Collections.Generic;
using Copious.Foundation;

namespace Copious.Persistance.Interface {
    public interface IQueryHandlerFactory {
        IEnumerable<IQueryHandlerAsync<TQuery, TQueryResult>> GetAsyncHandlers<TQuery, TQueryResult> () where TQuery : Query;

        IEnumerable<IQueryHandler<TQuery, TQueryResult>> GetHandlers<TQuery, TQueryResult> () where TQuery : Query;
    }
}