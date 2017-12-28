using Copious.Persistance.Interface;
using Copious.Foundation;
using Copious.Foundation.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Copious.Persistance.EF
{
    //Todo: Implment security column security (select) / row security (where) using the Query Guard

    public class GeneralQueryHandler<TState> : EFBase<TState>,
             IQueryHandler<GetAllQuery, List<TState>>,
             IQueryHandler<FindByIdQuery, TState>,
             IQueryHandler<SearchQuery<TState>, List<TState>>,

             IQueryHandlerAsync<GetAllQuery, List<TState>>,
             IQueryHandlerAsync<FindByIdQuery, TState>,
             IQueryHandlerAsync<SearchQuery<TState>, List<TState>>

        where TState : class, IUnique, new()
    {
        
        protected readonly IQueryGuard _guard;
        protected readonly IReadonlyRepository<TState> _readonlyRepository;

        public GeneralQueryHandler(DbContext dbContext, IQueryGuard guard) : base(dbContext)
        {
            _guard = guard;
            _readonlyRepository = new ReadonlyRepository<TState>(dbContext);
        }


        protected virtual List<TResult> Page<TResult>(Query query, IQueryable<TResult> queryable)
        {
            queryable = Sort(query, queryable);
            query.TotalItems = queryable.Count();
            return queryable.Page(query.PageNo, query.PageSize).ToList();
        }

        protected virtual IQueryable<TResult> Sort<TResult>(Query query, IQueryable<TResult> queryable) 
            => (query.SortProperties != null) ?
               queryable.OrderBy(string.Join(",", query.SortProperties.Select(s => $"{s.Key} {s.Value}"))) : queryable;

        public List<TState> Fetch(GetAllQuery query)
           => ProtectedResult(DbSet.ToList());

        public TState Fetch(FindByIdQuery query) 
            => _readonlyRepository.Get(query.Id);

        public List<TState> Fetch(SearchQuery<TState> query)
            => ProtectedResult(DynamicLinqHelper.ApplyFeatures(query, DbSet.AsNoTracking().Where(query.Predicate), _guard).ToList());

        public async Task<List<TState>> FetchAsync(GetAllQuery query)
            => ProtectedResult(await _readonlyRepository.GetAllAsync());

        public async Task<TState> FetchAsync(FindByIdQuery query)
            => await DbSet.SingleOrDefaultAsync(i => i.Id == query.Id);

        public async Task<List<TState>> FetchAsync(SearchQuery<TState> query)
            => ProtectedResult(await DynamicLinqHelper.ApplyFeatures(query, DbSet.AsNoTracking().Where(query.Predicate), _guard).ToListAsync());

        //todo: protect the select columns if not done in DynamicLinqHelper
        private static List<TState> ProtectedResult(List<TState> listToProtect) => listToProtect;
    }
}