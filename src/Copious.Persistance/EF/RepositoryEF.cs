using Copious.Foundation;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using Copious.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Copious.Persistance
{
    /// <summary>
    /// Generic Repository for entity framework implements both read and write
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class RepositoryEF<TState> :
        IRepository<TState>, // implmented because this class provides repo fns for write operations
        IQueryRepository<TState> // implmented because this class provides repo fns for read operations
        where TState : CopiousEntity, new()
    {
        protected readonly DbContext _context;

        public RepositoryEF(DbContext context)
        {
            _context = context;
        }

        private DbSet<TState> _dbSet;

        public DbSet<TState> DbSet
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = _context.Set<TState>();

                return _dbSet;
            }
        }

        public virtual List<TState> GetAll()
             => DbSet.ToList();

        public virtual async Task<List<TState>> GetAllAsync()
            => await DbSet.ToListAsync();

        public virtual IAsyncEnumerable<TState> GetAllAsAsync()
            => DbSet.ToAsyncEnumerable();

        public TState Get(Guid id)
            => DbSet.SingleOrDefault(i => i.Id == id);

        public int GetVersion(Guid id)
           => DbSet.Where(i => i.Id == id).Select(v => v.Version).SingleOrDefault();

        private bool Exists(Guid id)
            => DbSet.Any(i => i.Id == id);

        /// <summary>
        /// For EF there will be atmost only 1 event at a time
        /// </summary>
        /// <param name="aggId"></param>
        /// <param name="expectedVersion"></param>
        /// <param name="t">The actual state that will be persisted</param>
        /// <param name="events">Used to find out the data operation to do like insert / update / delete</param>
        public virtual void Save(Guid aggId, int expectedVersion, TState t, IEnumerable<Event> events)
        {
            Action<TState, int> dbOpr = null;
            //Check for crud events
            if (events.Any()) dbOpr = GetOperation(events.Single());
            // Determine the operation based on record existance
            if (dbOpr == null) if (Exists(aggId)) dbOpr = Insert; else dbOpr = Update;
            dbOpr?.Invoke(t, expectedVersion);
        }

        public virtual async Task SaveAsync(Guid aggId, int expectedVersion, TState t, IEnumerable<Event> events)
        {
            Func<TState, int, Task> dbOpr = null;
            //Check for crud events
            if (events.Any()) dbOpr = GetAsyncOperation(events.Single());
            // Determine the operation based on record existance
            if (dbOpr == null) if (Exists(aggId)) dbOpr = InsertAsync; else dbOpr = UpdateAsync;
            await dbOpr?.Invoke(t, expectedVersion);
        }

        private void Insert(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Add(t);
            _context.SaveChanges();
        }

        private async Task InsertAsync(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Add(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private void Update(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Update(t);
            _context.SaveChanges();
        }

        private async Task UpdateAsync(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Update(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private void Delete(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Remove(t);
            _context.SaveChanges();
        }

        private async Task DeleteAsync(TState t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Remove(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private Func<TState, int, Task> GetAsyncOperation(Event evt)
        {
            switch (evt)
            {
                case Created<TState> _:
                    return InsertAsync;

                case Deleted<TState> _:
                    return DeleteAsync;

                case Updated<TState> _:
                    return UpdateAsync;

                default:
                    return null;
            }
        }

        private Action<TState, int> GetOperation(Event evt)
        {
            switch (evt)
            {
                case Created<TState> _:
                    return Insert;

                case Deleted<TState> _:
                    return Delete;

                case Updated<TState> _:
                    return Update;

                default:
                    return null;
            }
        }

        private int CheckVersion(TState t, int expectedVersion)
        {
            var currentStateVersion = GetVersion(t.Id);

            //Version Check 2 - to verify whether there is any modification in between the first fetch in command handler and this call
            if (currentStateVersion != expectedVersion)
                throw new VersionConflictException(expectedVersion, t.Version);

            return ++currentStateVersion;
        }
    }
}