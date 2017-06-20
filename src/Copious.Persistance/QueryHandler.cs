using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;

namespace Copious.Persistance
{
    //Todo: Implment security column security (select) / row security (where) using the Query Guard
    public class QueryHandler
    {
        protected readonly IQueryGuard _guard;

        public QueryHandler(IQueryGuard guard)
        {
            _guard = guard;
        }

        protected virtual List<TResult> Page<TResult>(Query query, IQueryable<TResult> queryable)
        {
            queryable = Sort(query, queryable);
            query.TotalItems = queryable.Count();
            return queryable.Page(query.PageNo, query.PageSize).ToList();
        }

        protected virtual IQueryable<TResult> Sort<TResult>(Query query, IQueryable<TResult> queryable) =>
            (query.SortProperties != null) ?
            queryable.OrderBy(string.Join(",", query.SortProperties.Select(s => $"{s.Key} {s.Value}"))) : queryable;
    }
}