using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Copious.Foundation;

namespace Copious.Persistance.Interface {
    public class GetAllQuery : Query {
        public GetAllQuery (Func<RequestContext> contextProvider) : base (contextProvider) { }
    }

    public class FindByIdQuery : Query {
        public FindByIdQuery (Func<RequestContext> contextProvider, Guid id) : base (contextProvider) {
            EntityId = id;
        }

        public Guid EntityId { get; set; }
    }

    public class SearchQuery<TState> : Query
    where TState : class, new () {

        public SearchQuery (Func<RequestContext> contextProvider, Expression<Func<TState, bool>> predicate) : base (contextProvider) {
            Predicate = predicate;
            SearchMode = SearchMode.Search;
        }

        public SearchQuery (Func<RequestContext> contextProvider, string filterId, TState filterValue) : base (contextProvider) {
            FilterId = filterId;
            FilterValue = filterValue;
            SearchMode = SearchMode.Filter;
        }

        public Expression<Func<TState, bool>> Predicate { get; }
        public string FilterId { get; }
        public TState FilterValue { get; }

        public SearchMode SearchMode { get; }
    }

    public enum SearchMode {
        Search,
        Filter
    }
}