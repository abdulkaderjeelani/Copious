using Copious.Foundation;

namespace Copious.Persistance.Interface
{
    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TQuery, out TQueryResult> : IQueryHandler
        where TQuery : Query<TQueryResult>

    {
        TQueryResult Fetch(TQuery query);
    }
}