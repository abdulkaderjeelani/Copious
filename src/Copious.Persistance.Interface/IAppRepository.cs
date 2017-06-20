using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Persistance.Interface
{
    /// <summary>
    /// Repository for our application
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IAppRepository<TState> : IRepository<TState>, IQueryRepository<TState> where TState : class, IEntity, new()
    {
        Task<List<TState>> GetAllAsync(Guid systemId, StateStatus status = StateStatus.Active);

        List<TState> GetAll(Guid systemId, StateStatus status = StateStatus.Active);
    }
}