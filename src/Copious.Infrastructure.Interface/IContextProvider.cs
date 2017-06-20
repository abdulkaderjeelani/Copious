using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Infrastructure.Interface
{
    public interface IContextProvider
    {
        Context Context { get; }
    }
}