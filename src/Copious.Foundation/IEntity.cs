using System;
using System.Collections.Generic;
using Copious.Foundation.ComponentModel;

namespace Copious.Foundation {
    public interface IEntity<TKey> : IComponent<TKey> {
        IEnumerable<string> GetProperties ();
    }

}