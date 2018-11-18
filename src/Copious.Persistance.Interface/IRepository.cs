using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copious.Foundation;
using Copious.Foundation.ComponentModel;

namespace Copious.Persistance.Interface {
    public interface IRepository { }

    /// <summary>
    /// Query handlers will only inject this interface to its constructor, so it cannot get access to methods that changes state
    /// </summary>
    /// <typeparam name="TState">DTO for query, DataModel for commands</typeparam>
    public interface IReadonlyRepository<TState> : IRepository where TState : class, Identifiable<Guid>, new () {
        List<TState> GetAll ();

        Task<List<TState>> GetAllAsync ();

        IAsyncEnumerable<TState> GetAllAsAsync ();

        TState Get (Guid id);
    }

    public interface IRepository<TEntity> : IReadonlyRepository<TEntity> where TEntity : class, IEntity<Guid>, new () {

        void Save (Guid aggId, int expectedVersion, TEntity t, IEnumerable<Event> events);

        Task SaveAsync (Guid aggId, int expectedVersion, TEntity t, IEnumerable<Event> events);
    }
}