using System.Linq;
using System.Linq.Dynamic.Core;
using Copious.Foundation;
using Copious.Persistance.Interface;

namespace Copious.Persistance {
    public static class DynamicLinqHelper {
        /// <summary>
        /// Applies features like sort, page, security (row / col) to the given query
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryable"></param>
        /// <param name="guard"></param>
        /// <returns></returns>
        public static IQueryable<TResult> ApplyFeatures<TResult> (Query query, IQueryable<TResult> queryable, IQueryGuard guard) {
            queryable = Sort (query, queryable);

            if (query.PageNo > 0 && query.PageSize > 0)
                queryable = queryable.Page (query.PageNo, query.PageSize);

            query.TotalItems = queryable.Count ();

            return queryable;
        }

        public static IQueryable<TResult> Sort<TResult> (Query query, IQueryable<TResult> queryable) => (query.SortProperties != null) ? queryable.OrderBy (string.Join (",", query.SortProperties.Select (s => $"{s.Key} {s.Value}"))) : queryable;
    }
}