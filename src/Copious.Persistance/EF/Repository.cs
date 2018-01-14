using Copious.Foundation;
using Copious.Foundation.ComponentModel;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using Copious.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Copious.Persistance.EF
{

    /// <summary>
    /// Generic Repository for entity framework implements both read and write
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> :
        ReadonlyRepository<TEntity>,
        IRepository<TEntity> // implmented because this class provides repo fns for write operations

        where TEntity : CopiousEntity, new()
    {
        public Repository(DbContext context) : base(context)
        {

        }
                
        public int GetVersion(Guid id)
           => DbSet.Where(i => i.Id == id).Select(v => v.Version).SingleOrDefault();

        bool Exists(Guid id)
            => DbSet.Any(i => i.Id == id);

        /// <summary>
        /// For EF there will be atmost only 1 event at a time
        /// </summary>
        /// <param name="aggId"></param>
        /// <param name="expectedVersion"></param>
        /// <param name="t">The actual state that will be persisted</param>
        /// <param name="events">Used to find out the data operation to do like insert / update / delete</param>
        public virtual void Save(Guid aggId, int expectedVersion, TEntity t, IEnumerable<Event> events)
        {
            Action<TEntity, int> dbOpr = null;
            //Check for crud events
            if (events.Any()) dbOpr = GetOperation(events.Single());
            // Determine the operation based on record existance
            if (dbOpr == null) if (Exists(aggId)) dbOpr = Insert; else dbOpr = Update;
            dbOpr?.Invoke(t, expectedVersion);
        }

        public virtual async Task SaveAsync(Guid aggId, int expectedVersion, TEntity t, IEnumerable<Event> events)
        {
            Func<TEntity, int, Task> dbOpr = null;
            //Check for crud events
            if (events.Any()) dbOpr = GetAsyncOperation(events.Single());
            // Determine the operation based on record existance
            if (dbOpr == null) if (Exists(aggId)) dbOpr = InsertAsync; else dbOpr = UpdateAsync;
            await dbOpr?.Invoke(t, expectedVersion);
        }

        void Insert(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Add(t);
            _context.SaveChanges();
        }

        async Task InsertAsync(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Add(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        void Update(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Update(t);
            _context.SaveChanges();
        }

        async Task UpdateAsync(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Update(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        void Delete(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Remove(t);
            _context.SaveChanges();
        }

        async Task DeleteAsync(TEntity t, int expectedVersion)
        {
            t.Version = CheckVersion(t, expectedVersion);
            DbSet.Remove(t);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        Func<TEntity, int, Task> GetAsyncOperation(Event evt)
        {
            switch (evt)
            {
                case Created<TEntity> _:
                    return InsertAsync;

                case Deleted<TEntity> _:
                    return DeleteAsync;

                case Updated<TEntity> _:
                    return UpdateAsync;

                default:
                    return null;
            }
        }

        Action<TEntity, int> GetOperation(Event evt)
        {
            switch (evt)
            {
                case Created<TEntity> _:
                    return Insert;

                case Deleted<TEntity> _:
                    return Delete;

                case Updated<TEntity> _:
                    return Update;

                default:
                    return null;
            }
        }

        int CheckVersion(TEntity t, int expectedVersion)
        {
            var currentStateVersion = GetVersion(t.Id);

            //Version Check 2 - to verify whether there is any modification in between the first fetch in command handler and this call
            if (currentStateVersion != expectedVersion)
                throw new VersionConflictException(expectedVersion, t.Version);

            return ++currentStateVersion;
        }
    }
}