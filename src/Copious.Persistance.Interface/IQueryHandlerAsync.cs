using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Persistance.Interface
{
    public interface IQueryHandlerAsync<TQuery, TQueryResult> : IQueryHandler
     where TQuery : Query<TQueryResult>

    {
        Task<TQueryResult> FetchAsync(TQuery query);
    }
}