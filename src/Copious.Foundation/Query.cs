using Copious.Foundation.ComponentModel;
using System;
using System.Collections.Generic;

namespace Copious.Foundation
{
    public class Query : Operation
    {
        public Guid SystemId { get; set; } = new Guid("1B421466-EE83-4C36-81B1-1338DBAD6D8F");

        public string UserId { get; set; }

        public virtual Dictionary<string, SortDirection> SortProperties { get; set; }

        public virtual int PageNo { get; set; }
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Query handler after paging set this property value, so that the caller can read this
        /// </summary>
        public virtual int TotalItems { get; set; }
    }

    public class Query<TQueryResult> : Query, IFilterable
    {
        public Query()
        {
        }

        public Query(Context context) => Context = context;
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }

    #region Query Marker Interfaces

    /// <summary>
    /// By default all queries are filterable,
    /// </summary>
    public interface IFilterable
    {
    }

    public interface ISortable
    {
    }

    public interface IPageable
    {
    }

    #endregion Query Marker Interfaces
}