using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface
{
    public interface IQueryProcessor
    {
        TQueryResult Process<TQueryResult>(Query<TQueryResult> query);

        TQueryResult Process<TQuery, TQueryResult>(TQuery query) where TQuery : Query<TQueryResult>;

        Task<TQueryResult> ProcessAsync<TQueryResult>(Query<TQueryResult> query);

        Task<TQueryResult> ProcessAsync<TQuery, TQueryResult>(TQuery query) where TQuery : Query<TQueryResult>;
    }
}