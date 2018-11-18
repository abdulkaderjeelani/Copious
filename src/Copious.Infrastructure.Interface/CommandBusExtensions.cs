using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface {
    /// <summary>
    /// Provides usability overloads for <see cref="ICommandBus"/>
    /// </summary>
    public static class CommandBusExtensions {
        public static void Send<TCommand> (this ICommandBus bus, TCommand command) where TCommand : Command => bus.Send (new Envelope<TCommand> (command));

        public static async Task SendAsAsync<TCommand> (this ICommandBus bus, TCommand command) where TCommand : Command => await bus.SendAsAsync (new Envelope<TCommand> (command));

        public static async Task SendAsync<TCommand> (this ICommandBus bus, TCommand command) where TCommand : Command => await bus.SendAsync (new Envelope<TCommand> (command));

        public static void SendAsyncAsSync<TCommand> (this ICommandBus bus, TCommand command) where TCommand : Command => bus.SendAsyncAsSync (new Envelope<TCommand> (command));
    }
}