using Copious.Application.Interface;
using Copious.Infrastructure.Interface.Services;

namespace Copious.Application
{
    public class CommandGuard : ICommandGuard
    {
        private readonly ISecurityProvider _securityProvider;

        public CommandGuard(ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
        }
    }
}