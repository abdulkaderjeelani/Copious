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

    public class ReadonlyRepository<TState> :
        EFBase<TState>,
        IReadonlyRepository<TState> // implmented because this class provides repo fns for read operations
        where TState : class, IUnique, new()
    {
        public ReadonlyRepository(DbContext context) : base(context)
        {
            
        }

        public virtual List<TState> GetAll()
            => DbSet.ToList();

        public virtual async Task<List<TState>> GetAllAsync()
            => await DbSet.ToListAsync();

        public virtual IAsyncEnumerable<TState> GetAllAsAsync()
            => DbSet.ToAsyncEnumerable();

        public TState Get(Guid id)
            => DbSet.SingleOrDefault(i => i.Id == id);
    }

}