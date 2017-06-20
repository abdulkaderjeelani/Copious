using Copious.Foundation;

namespace Copious.Infrastructure.AspNet.Model
{
    public class QueryModel<TQuery, TQueryResult> where TQuery : Query<TQueryResult>
    {
        public TQuery Query { get; set; }
    }
}