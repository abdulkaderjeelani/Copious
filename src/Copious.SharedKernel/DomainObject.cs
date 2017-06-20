using Copious.Foundation;

namespace Copious.SharedKernel
{
    /// <summary>
    /// Domain ojbect of our application, All domain object must dervice from this either directly or indirectly
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TObjectState"></typeparam>
    public abstract class DomainObject<TDomainObject, TObjectState> : StatedAggregate<TDomainObject, TObjectState>, IEntity
     where TObjectState : CopiousEntity, new()
    {
        protected DomainObject()
        {
        }

        protected DomainObject(TObjectState state) : base(state)
        {
        }
    }
}