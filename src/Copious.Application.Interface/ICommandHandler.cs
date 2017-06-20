using Copious.Foundation;

namespace Copious.Application.Interface
{
    /// <summary>
    /// Marker interface for command handlers
    /// </summary>
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : Command
    {
        void Execute(TCommand command);
    }
}