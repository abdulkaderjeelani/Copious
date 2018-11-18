using System;
using Copious.Foundation;

namespace Copious.SharedKernel {
    /// <summary>
    /// Domain ojbect of our application, All domain object must derive from this either directly or indirectly
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TObjectState"></typeparam>
    public abstract class DomainObject<TDomainObject, TObjectState> : StatedAggregate<TDomainObject, TObjectState>, IEntity<Guid>
        where TObjectState : CopiousEntity, new () {
            protected DomainObject () { }

            protected DomainObject (TObjectState state) : base (state) { }
        }
}