using System;
using System.Collections.Generic;
using System.Text;
using Copious.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Copious.Persistance {
    public class DbOptions {
        public string ConnectionStringKey { get; set; }

        public string MigrationsAssembly { get; set; }

        public Db DbToUse { get; set; }

        public bool IsIdentityDb { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }

    public class DbOptions<TContext> : DbOptions where TContext : DbContext {
        public Action<TContext, bool> OnSeeding { get; set; }
    }

}