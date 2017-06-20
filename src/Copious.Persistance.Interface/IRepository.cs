using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Copious.Persistance.Interface
{
    public interface IRepository
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">DTO for query, DataModel for commands</typeparam>
    public interface IQueryRepository<T> : IRepository where T : class, IEntity, new()
    {
        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        IAsyncEnumerable<T> GetAllAsAsync();
    }

    public interface IRepository<TState> : IRepository where TState : class, IEntity, new()
    {
        TState Get(Guid id);

        void Save(Guid aggId, int expectedVersion, TState t, IEnumerable<Event> events);

        Task SaveAsync(Guid aggId, int expectedVersion, TState t, IEnumerable<Event> events);
    }
}