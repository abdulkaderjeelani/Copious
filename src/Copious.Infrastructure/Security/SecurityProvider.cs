using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Infrastructure.Security
{
    public class SecurityProvider : ISecurityProvider
    {
        readonly IContextProvider _contextProvider;

        public SecurityProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }
    }
}