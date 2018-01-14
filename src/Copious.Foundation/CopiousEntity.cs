using Copious.Foundation;
using System;

namespace Copious.Foundation
{
    /// <summary>
    /// Base entity of our application, All state classes must derive from this directly or indirectly (through module's state)
    /// </summary>
    [Serializable]
    public abstract class CopiousEntity : Entity<Guid>
    {
        public virtual Guid SystemId { get; set; }

        protected CopiousEntity(Guid id) : base(id)
        {
        }

        protected CopiousEntity()
        {
        }
    }
}