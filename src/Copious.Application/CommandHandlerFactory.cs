using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Application {
    public sealed class CommandHandlerFactory : ICommandHandlerFactory {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerFactory (IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ICommandHandlerAsync<T>> GetAsyncHandlers<T> () where T : Command => MessageHandlerResolver.GetMessageHandlerType<T> (typeof (ICommandHandlerAsync<>))
            .Select (h => (ICommandHandlerAsync<T>) GetCommandHandlerInstance (h));

        public IEnumerable<ICommandHandler<T>> GetHandlers<T> () where T : Command => MessageHandlerResolver.GetMessageHandlerType<T> (typeof (ICommandHandler<>))
            .Select (h => (ICommandHandler<T>) GetCommandHandlerInstance (h));

        private object GetCommandHandlerInstance (Type h) => ActivatorUtilities.CreateInstance (_serviceProvider, h);

    }
}