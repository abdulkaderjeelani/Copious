using Copious.Foundation;

namespace Copious.Infrastructure.AspNet.Model {
    public class QueryModel<TQuery> where TQuery : Query {
        public TQuery Query { get; set; }
    }
}