﻿using System;
using System.Collections.Generic;

namespace Copious.Foundation
{
    public class CrudCommand<TState> : Command<TState>
        where TState : class, IEntity<Guid>, new()
    {
        public CrudCommand(TState state, int version) : base(state, state.Id, version)
        {
        }

        public CrudCommand(Guid stateId, int version) : base(new TState(), stateId, version)
        {
            State.Id = stateId;
        }
    }

    public class Create<TState> : CrudCommand<TState>
        where TState : class, IEntity<Guid>, new()
    {
        public Create(TState state, int version) : base(state, version)
        {
        }
    }

    public class Update<TState> : CrudCommand<TState>
       where TState : class, IEntity<Guid>, new()
    {
        public Update(TState state, int version) : base(state, version)
        {
        }

        public Update(TState state, int version, IEnumerable<string> propertiesToUpdate) : base(state, version)
        {
            PropertiesToUpdate = propertiesToUpdate;
        }

        public IEnumerable<string> PropertiesToUpdate { get; set; }

        /// <summary>
        /// Unique name to identify the update command.
        /// E.g. if an aggregate has many updates each for different scenario with different properties, we name them.
        /// We can also populate PropertiesToUpdate based on name (from configuration, we shall have a setting like name - properties map and read it from directly)
        /// </summary>
        public string UpdateProcessName { get; set; } = "Default";
    }

    public class Delete<TState> : CrudCommand<TState>
      where TState : class, IEntity<Guid>, new()
    {
        public Delete(TState state, int version) : base(state, version)
        {
        }

        public Delete(Guid stateId, int version) : base(stateId, version)
        {
        }
    }
}