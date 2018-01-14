using System;

using Copious.Foundation.ComponentModel;

namespace Copious.Foundation
{
    public class Operation : Component
    {

        RequestContext _context;
        Func<RequestContext> _contextProvider;

        public RequestContext Context => _contextProvider?.Invoke() ?? _context;

        public void SetContext(RequestContext context) => _context = context;

        public void SetContext(Func<RequestContext> contextProvider) => _contextProvider = contextProvider;


    }
}