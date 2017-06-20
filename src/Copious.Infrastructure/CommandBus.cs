using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;

namespace Copious.Infrastructure
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandHandlerFactory _handlerFactory;

        public CommandBus(IServiceProvider serviceProvider, ICommandHandlerFactory handlerFactory)
        {
            _serviceProvider = serviceProvider;
            _handlerFactory = handlerFactory;
        }

        [DebuggerStepThrough]
        public void Send<T>(Envelope<T> command) where T : Command
        {
            var syncHandler = _serviceProvider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;

            if (syncHandler == null)
                syncHandler = _handlerFactory.GetHandlers<T>()?.FirstOrDefault() ?? null;

            if (syncHandler == null)
                throw new KeyNotFoundException("Handler not found");

            syncHandler.Execute(command.Body);
        }

        [DebuggerStepThrough]
        public Task SendAsAsync<T>(Envelope<T> command) where T : Command
          => Task.Factory.StartNew(() => Send(command));

        [DebuggerStepThrough]
        public async Task SendAsync<T>(Envelope<T> command) where T : Command
        {
            var asyncHandler = _serviceProvider.GetService(typeof(ICommandHandlerAsync<T>)) as ICommandHandlerAsync<T>;

            if (asyncHandler == null)
                asyncHandler = _handlerFactory.GetAsyncHandlers<T>()?.FirstOrDefault() ?? null;

            if (asyncHandler == null)
                throw new KeyNotFoundException("Handler not found");

            await asyncHandler.ExecuteAsync(command.Body);
        }

        // http://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c
        [DebuggerStepThrough]
        public void SendAsyncAsSync<T>(Envelope<T> command) where T : Command
           => Task.Run(async () => await SendAsync(command)).Wait();
    }
}