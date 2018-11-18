using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface {
    public interface IQueryProcessor {
        TQueryResult Process<TQuery, TQueryResult> (TQuery query) where TQuery : Query;

        Task<TQueryResult> ProcessAsync<TQuery, TQueryResult> (TQuery query) where TQuery : Query;

    }
}