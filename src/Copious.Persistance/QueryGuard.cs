using System;
using System.Collections.Generic;
using System.Text;
using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.Persistance.Interface;

namespace Copious.Persistance {
    public class QueryGuard : IQueryGuard {
        readonly ISecurityProvider _securityProvider;

        public QueryGuard (ISecurityProvider securityProvider) {
            _securityProvider = securityProvider;
        }
    }
}