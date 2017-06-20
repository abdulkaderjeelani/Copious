namespace Copious.Foundation.ComponentModel
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Component : IComponent
    {
        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }

        [NotMapped]
        public virtual string CompName => GetType().Name;
    }
}