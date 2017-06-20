using System.Collections.Generic;
using System.Threading.Tasks;
using Copious.Persistance.Interface;
using Copious.Foundation;

namespace Copious.Persistance
{
    /// <summary>
    /// Repository Query Handler will use the reposiotry to access the database,
    /// so that we have extra layer of asbstraction, repository may use any mechanism to store the data
    /// that we dont care about
    /// </summary>
    public class BaseRepositoryQueryHandler<TEntity> : QueryHandler where TEntity : CopiousEntity, new()
    {
        protected readonly IQueryRepository<TEntity> _repository;

        public BaseRepositoryQueryHandler(IQueryRepository<TEntity> repository, IQueryGuard guard) : base(guard)
        {
            _repository = repository;
        }
    }

    public abstract class RepositoryQueryHandler<TEntity> : BaseRepositoryQueryHandler<TEntity>,
                 IQueryHandler<GetAllQuery<TEntity>, List<TEntity>>,
                 IQueryHandler<FindByIdQuery<TEntity>, TEntity>,
                 IQueryHandler<SearchQuery<TEntity>, List<TEntity>>
        where TEntity : CopiousEntity, new()
    {
        protected RepositoryQueryHandler(IQueryRepository<TEntity> repository, IQueryGuard guard) : base(repository, guard)
        {
        }

        public abstract TEntity Fetch(FindByIdQuery<TEntity> query);

        public abstract List<TEntity> Fetch(GetAllQuery<TEntity> query);

        public abstract List<TEntity> Fetch(SearchQuery<TEntity> query);
    }

    public abstract class RepositoryAsyncQueryHandler<TEntity> : BaseRepositoryQueryHandler<TEntity>,
                 IQueryHandlerAsync<GetAllQuery<TEntity>, List<TEntity>>,
                 IQueryHandlerAsync<FindByIdQuery<TEntity>, TEntity>,
                 IQueryHandlerAsync<SearchQuery<TEntity>, List<TEntity>>
       where TEntity : CopiousEntity, new()
    {
        protected RepositoryAsyncQueryHandler(IQueryRepository<TEntity> repository, IQueryGuard guard) : base(repository, guard)
        {
        }

        public Task<List<TEntity>> FetchAsync(SearchQuery<TEntity> query) => null;

        public virtual Task<TEntity> FetchAsync(FindByIdQuery<TEntity> query) => null;

        public virtual async Task<List<TEntity>> FetchAsync(GetAllQuery<TEntity> query)
            => await _repository.GetAllAsync();
    }
}