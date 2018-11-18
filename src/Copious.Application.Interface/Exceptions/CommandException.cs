using System;

namespace Copious.Application.Interface.Exceptions {
    /// <summary>
    ///     Exception to be thrown, if a command invoker is to be notified with useful data.
    /// </summary>
    public class CommandException : Exception {
        public CommandException () { }

        public CommandException (string message) : base (message) { }

        public CommandException (string message, Exception innerException) : base (message, innerException) { }

        public override string ToString () {
            return $"{Message} {base.ToString()}";
        }
    }
}