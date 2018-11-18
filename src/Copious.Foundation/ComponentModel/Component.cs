namespace Copious.Foundation.ComponentModel {
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    public class Component<TKey> : IComponent<TKey> {
        public virtual TKey Id { get; set; }

        public virtual int Version { get; set; }

        [NotMapped]
        public virtual string CompName => GetType ().Name;
    }

    public class Component : Component<Guid> {

    }
}