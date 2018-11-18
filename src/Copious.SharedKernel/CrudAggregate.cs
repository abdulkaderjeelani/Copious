using System;
using System.Linq;
using AutoMapper;
using Copious.Foundation;

namespace Copious.SharedKernel {
    public class CrudAggregate<TState> : StatedAggregate<CrudAggregate<TState>, TState>,
        IHandleCommand<Create<TState>>,
        IHandleCommand<Update<TState>>,
        IHandleCommand<Delete<TState>>,
        IApplyDomainEvent<Created<TState>>,
        IApplyDomainEvent<Updated<TState>>,
        IApplyDomainEvent<Deleted<TState>>
        where TState : class, IEntity<Guid>, new () {
            /// <summary>
            /// Override to perform any validations during handle and call the base on valid branch;
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            public virtual void Handle (Create<TState> command) => Produce (new Created<TState> (command.State, false));

            public virtual void Handle (Update<TState> command) => Produce (new Updated<TState> (command.State, false));

            public virtual void Handle (Delete<TState> command) => Produce (new Deleted<TState> (command.State, false));

            public virtual void Apply (Created<TState> @event) => State = @event.State;

            public virtual void Apply (Updated<TState> @event) => ChangeState (State, @event);

            /// <summary>
            /// Dont change state for delete,
            /// </summary>        
            /// <param name="event">event parameter on Apply</param>
            public virtual void Apply (Deleted<TState> @event) { }

            void ChangeState (TState actual, Updated<TState> @event) {
                //source state is set in command handler, modified state is payload of the command
                var modified = @event.State;
                if (@event.PropertiesToBeUpdated != null && @event.PropertiesToBeUpdated.Any ()) {
                    var specificConfig = new MapperConfiguration (config => {
                        var map = config.CreateMap<TState, TState> ();
                        foreach (var prop in modified.GetProperties ())
                            if (!@event.PropertiesToBeUpdated.Contains (prop))
                                map.ForMember (prop, opt => opt.Ignore ());
                    });
                    specificConfig.CreateMapper ().Map (modified, actual);
                    return;
                }

                Mapper.Map<TState, TState> (modified, actual);
            }
        }
}