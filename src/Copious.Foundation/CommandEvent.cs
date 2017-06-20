using Copious.Foundation;

namespace Copious.Foundation
{
    public class CommandEvent<TCommand> : Event
        where TCommand : Command
    {
        public CommandEvent(TCommand command) : base(command.ComponentId, command.ComponentVersion)
        {
            Command = command;
        }

        public TCommand Command { get; }
    }
}