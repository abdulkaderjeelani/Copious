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

    public abstract class EFBase<TState> where TState : class, IUnique, new()
    {
        protected readonly DbContext _context;

        protected EFBase(DbContext context)
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

    }

}