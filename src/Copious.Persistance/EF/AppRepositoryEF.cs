using Copious.Persistance.Interface;
using Copious.Foundation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Copious.Persistance
{
    /// <summary>
    /// Base repository for all our module repositories, in other words, Any repo will inherit from this class
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public abstract class AppRepositoryEF<TState> : RepositoryEF<TState>,
        IAppRepository<TState> // implmented because this class is part of our application
        where TState : CopiousEntity, new()
    {
        protected AppRepositoryEF(DbContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<List<TState>> GetAllAsync(Guid systemId, StateStatus status = StateStatus.Active)
            => await DbSet.AsNoTracking().Where(e => e.SystemId == systemId).ToListAsync();

        public virtual List<TState> GetAll(Guid systemId, StateStatus status = StateStatus.Active)
            => DbSet.AsNoTracking().Where(e => e.SystemId == systemId).ToList();
    }
}