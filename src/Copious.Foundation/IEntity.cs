using System;
using System.Collections.Generic;
using Copious.Foundation.ComponentModel;

namespace Copious.Foundation
{
    public interface IEntity : IComponent
    {
        IEnumerable<string> GetProperties();
    }
}