using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Persistance.Interface {
    public interface IQueryHandlerAsync<in TQuery, TQueryResult> : IQueryHandler
    where TQuery : Query

    {
        Task<TQueryResult> FetchAsync (TQuery query);
    }
}