using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Copious.Foundation;

namespace Copious.Persistance.Interface
{
    public class GetAllQuery : Query        
    {        
        public GetAllQuery(Context context) : base(context)
            => Context = context;
    }    

    public class FindByIdQuery : Query       
    {
        public FindByIdQuery(Context context) : base(context)
            => Context = context;

        public FindByIdQuery(Context context, Guid id) : this(context)
        {
            EntityId = id;
        }

        public Guid EntityId { get; set; }
    }

    public class SearchQuery<TState> : Query
        where TState : class, new()
    {
        public SearchQuery(Context context) : base(context)
            => Context = context;

        public SearchQuery(Context context, Expression<Func<TState, bool>> predicate) : this(context)
        {
            Predicate = predicate;
            SearchMode = SearchMode.Search;
        }

        public SearchQuery(Context context, string filterId, TState filterValue) : this(context)
        {
            FilterId = filterId;
            FilterValue = filterValue;
            SearchMode = SearchMode.Filter;
        }

        public Expression<Func<TState, bool>> Predicate { get; }
        public string FilterId { get; }
        public TState FilterValue { get; }

        public SearchMode SearchMode { get; }
    }
    
    public enum SearchMode
    {
        Search,
        Filter
    }
}