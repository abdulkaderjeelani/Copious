using Copious.Foundation;

namespace Copious.SharedKernel {
    /// <summary>
    /// Implement this interface on an Aggregate,
    /// In general Aggregate will handle command by performing validation on command (i.e. checking whether the command is valid)
    /// If valid then corresponding events are applied.
    /// </summary>
    public interface IHandleCommand {
        void Handle (Command command);
    }

    public interface IHandleCommand<in TCommand> where TCommand : Command {
        /*During bdd test return the events from the aggregate.changes*/

        void Handle (TCommand command);
    }
}