using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Foundation;
using Copious.Foundation.ComponentModel;

namespace Copious.Foundation
{
    [Serializable]
    public abstract class Entity : Component, IEntity, IEquatable<Entity>
    {
        private static readonly ConcurrentDictionary<string, string[]> EntityProperties = new ConcurrentDictionary<string, string[]>();

        protected Entity()
        {
        }

        protected Entity(Guid id)
        {
            if (Equals(id, default(Guid)))
                throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));

            SetId(id);
        }

        protected dynamic Self => this;

        public override bool Equals(object obj)
        {
            var entity = obj as Entity;
            return entity != null ? Equals(entity) : ReferenceEquals(this, obj);
        }

        public bool Equals(Entity other) => other != null && Id.Equals(other.Id);

        public override int GetHashCode() => Id.GetHashCode();

        public virtual IEnumerable<string> GetProperties()
        {
            var key = this.GetType().Name;
            if (!EntityProperties.TryGetValue(key, out string[] entityProperties))
                entityProperties = EntityProperties.GetOrAdd(key, this.GetType().GetTypeInfo().
                                   GetProperties().Select(p => p.Name).ToArray());

            //As we modify the returned data, copy it else the original cache may get modified due to reference type.
            Array array = new string[entityProperties.Length];
            entityProperties.CopyTo(array, 0);
            return array.Cast<string>();
        }

        private void SetId(Guid id) => Id = id;
    }
}