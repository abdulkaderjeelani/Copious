using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Copious.Foundation;

namespace Copious.Persistance.Interface
{
    public class GetAllQuery<TEntity> : Query<List<TEntity>>
        where TEntity : class, IEntity, new()
    {
        public GetAllQuery()
        {
        }

        public GetAllQuery(Context context) : base(context)
            => Context = context;
    }

    public class GetAllQueryAsync<TEntity> : Query<IAsyncEnumerable<TEntity>>
       where TEntity : class, IEntity, new()
    {
        public GetAllQueryAsync()
        {
        }

        public GetAllQueryAsync(Context context) : base(context)
            => Context = context;
    }

    public class FindByIdQuery<TEntity> : Query<TEntity>
        where TEntity : class, IEntity, new()
    {
        public FindByIdQuery()
        {
        }

        public FindByIdQuery(Context context) : base(context)
            => Context = context;

        public FindByIdQuery(Context context, Guid id) : this(context)
        {
            EntityId = id;
        }

        public Guid EntityId { get; set; }
    }

    public class SearchQuery<TEntity> : Query<List<TEntity>>
        where TEntity : class, IEntity, new()
    {
        public SearchQuery()
        {
        }

        public SearchQuery(Context context) : base(context)
            => Context = context;

        public SearchQuery(Context context, Expression<Func<TEntity, bool>> predicate) : this(context)
        {
            Predicate = predicate;
            SearchMode = SearchMode.Search;
        }

        public SearchQuery(Context context, string filterId, TEntity filterValue) : this(context)
        {
            FilterId = filterId;
            FilterValue = filterValue;
            SearchMode = SearchMode.Filter;
        }

        public Expression<Func<TEntity, bool>> Predicate { get; }
        public string FilterId { get; }
        public TEntity FilterValue { get; }

        public SearchMode SearchMode { get; }
    }

    public class SearchQueryAsync<TEntity> : Query<IAsyncEnumerable<TEntity>>
     where TEntity : class, IEntity, new()
    {
        public SearchQueryAsync()
        {
        }

        public SearchQueryAsync(Context context) : base(context)
            => Context = context;

        public SearchQueryAsync(Context context, Expression<Func<TEntity, bool>> predicate) : this(context)
        {
            Predicate = predicate;
            SearchMode = SearchMode.Search;
        }

        public SearchQueryAsync(Context context, string filterId, TEntity filterValue) : this(context)
        {
            FilterId = filterId;
            FilterValue = filterValue;
            SearchMode = SearchMode.Filter;
        }

        public Expression<Func<TEntity, bool>> Predicate { get; }

        public string FilterId { get; }
        public TEntity FilterValue { get; }
        public SearchMode SearchMode { get; }
    }

    public enum SearchMode
    {
        Search,
        Filter
    }
}