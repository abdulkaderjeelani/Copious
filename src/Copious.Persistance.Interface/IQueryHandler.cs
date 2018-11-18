using Copious.Foundation;

namespace Copious.Persistance.Interface {
    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IQueryHandler {

    }

    public interface IQueryHandler<in TQuery, out TQueryResult> : IQueryHandler
    where TQuery : Query

    {
        TQueryResult Fetch (TQuery query);
    }

}