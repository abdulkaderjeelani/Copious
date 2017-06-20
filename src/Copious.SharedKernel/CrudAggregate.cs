using AutoMapper;
using Copious.Foundation;
using System.Linq;

namespace Copious.SharedKernel
{
    public class CrudAggregate<TState> : StatedAggregate<CrudAggregate<TState>, TState>,
        IHandleCommand<Create<TState>>,
        IHandleCommand<Update<TState>>,
        IHandleCommand<Delete<TState>>,
        IApplyEvent<Created<TState>>,
        IApplyEvent<Updated<TState>>,
        IApplyEvent<Deleted<TState>>
        where TState : class, IEntity, new()
    {
        /// <summary>
        /// Override to perform any validations during handle and call the base on valid branch;
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual void Handle(Create<TState> command)
            => Produce(new Created<TState>(command.State, false));

        public virtual void Handle(Update<TState> command)
            => Produce(new Updated<TState>(command.State, false));

        public virtual void Handle(Delete<TState> command)
            => Produce(new Deleted<TState>(command.State, false));

        public virtual void Apply(Created<TState> e) => State = e.State;

        public virtual void Apply(Updated<TState> e) => ChangeState(State, e);

        /// <summary>
        /// Dont change state for delete,
        /// </summary>
        /// <param name="e"></param>
        public virtual void Apply(Deleted<TState> e) { }

        private void ChangeState(TState actual, Updated<TState> e)
        {
            //source state is set in command handler, modified state is payload of the command
            var modified = e.State;
            if (e.PropertiesToBeUpdated != null && e.PropertiesToBeUpdated.Any())
            {
                var specificConfig = new MapperConfiguration(config =>
                {
                    var map = config.CreateMap<TState, TState>();
                    foreach (var prop in modified.GetProperties())
                        if (!e.PropertiesToBeUpdated.Contains(prop))
                            map.ForMember(prop, opt => opt.Ignore());
                });
                specificConfig.CreateMapper().Map(modified, actual);
                return;
            }

            Mapper.Map(modified, actual);
        }
    }
}