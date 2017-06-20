using Copious.Persistance.Interface;
using Copious.Foundation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Copious.Persistance
{
    public abstract class AppQueryHandlerEF<TEntity> : QueryHandler,
             IQueryHandler<GetAllQuery<TEntity>, List<TEntity>>,
             IQueryHandler<FindByIdQuery<TEntity>, TEntity>,
             IQueryHandler<SearchQuery<TEntity>, List<TEntity>>,

             IQueryHandlerAsync<GetAllQuery<TEntity>, List<TEntity>>,
             IQueryHandlerAsync<FindByIdQuery<TEntity>, TEntity>,
             IQueryHandlerAsync<SearchQuery<TEntity>, List<TEntity>>

        where TEntity : CopiousEntity, new()
    {
        private readonly AppRepositoryEF<TEntity> _appRepositoryEF;
        private readonly DbContext _dbContext;

        protected AppQueryHandlerEF(DbContext dbContext, IAppRepository<TEntity> appRepositoryEF, IQueryGuard guard) : base(guard)
        {
            _dbContext = dbContext;
            _appRepositoryEF = (AppRepositoryEF<TEntity>)appRepositoryEF;
        }

        public List<TEntity> Fetch(GetAllQuery<TEntity> query)
           => ProtectedResult(_appRepositoryEF.DbSet.ToList());

        public TEntity Fetch(FindByIdQuery<TEntity> query)
            => _appRepositoryEF.Get(query.Id);

        public List<TEntity> Fetch(SearchQuery<TEntity> query)
            => ProtectedResult(DynamicLinqHelper.ApplyFeatures(query, _appRepositoryEF.DbSet.AsNoTracking().Where(query.Predicate), _guard).ToList());

        public async Task<List<TEntity>> FetchAsync(GetAllQuery<TEntity> query)
            => ProtectedResult(await _appRepositoryEF.GetAllAsync(query.SystemId));

        public async Task<TEntity> FetchAsync(FindByIdQuery<TEntity> query)
            => await _appRepositoryEF.DbSet.SingleOrDefaultAsync(i => i.Id == query.Id);

        public async Task<List<TEntity>> FetchAsync(SearchQuery<TEntity> query)
            => ProtectedResult(await DynamicLinqHelper.ApplyFeatures(query, _appRepositoryEF.DbSet.AsNoTracking().Where(query.Predicate), _guard).ToListAsync());

        //todo: protect the select columns if not done in DynamicLinqHelper
        private List<TEntity> ProtectedResult(List<TEntity> listToProtect) => listToProtect;
    }
}