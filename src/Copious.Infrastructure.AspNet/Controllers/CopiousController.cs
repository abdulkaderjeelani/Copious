using System.Linq;
using Copious.Application.Interface.Exceptions;
using Copious.Foundation;
using Copious.Infrastructure.AspNet.Results;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Copious.Infrastructure.AspNet.Controllers
{
    [Route("api/[controller]")]
    public class CopiousController : Controller
    {
        protected readonly IExceptionHandler _exceptionHandler;
        protected readonly IQueryProcessor _queryProcessor;
        protected readonly ICommandBus _commandBus;
        protected readonly IContextProvider _contextProvider;
        protected readonly ILogger _logger;

        public CopiousController()
        {
        }

        public CopiousController(IContextProvider contextProvider, IExceptionHandler exceptionHandler, ILoggerFactory loggerFactory)
        {
            _contextProvider = contextProvider;
            _exceptionHandler = exceptionHandler;
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        public CopiousController(IContextProvider contextProvider, IExceptionHandler exceptionHandler, IQueryProcessor queryProcessor, ICommandBus commandBus, ILoggerFactory loggerFactory)
            : this(contextProvider, exceptionHandler, loggerFactory)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        public CopiousController(IContextProvider contextProvider, IExceptionHandler exceptionHandler, IQueryProcessor queryProcessor, ILoggerFactory loggerFactory)
            : this(contextProvider, exceptionHandler, loggerFactory)
        {
            _queryProcessor = queryProcessor;
        }

        public CopiousController(IContextProvider contextProvider, IExceptionHandler exceptionHandler, ICommandBus commandBus, ILoggerFactory loggerFactory)
            : this(contextProvider, exceptionHandler, loggerFactory)
        {
            _commandBus = commandBus;
        }

        protected async Task ExecuteCommandAsync<T>(T command, ErrorCode errorCode, params Func<Task>[] failureHandlers)
            where T : Command
        {
            try
            {
                _logger.LogInformation("Execte command from controller " + command.GetType());

                AddContext(command);

                await _commandBus.SendAsync(command);
            }
            // Dont let command ex pass through, wrap it
            catch (CommandException ex)
            {
                await InvokeFailureHandlersAsync(failureHandlers);
                throw new ControllerException(ex, errorCode);
            }
            catch
            {
                await InvokeFailureHandlersAsync(failureHandlers);
                throw;
            }
        }

        protected async Task InvokeFailureHandlersAsync(params Func<Task>[] failureHandlers) =>
            //By default always run the failure hanlder in sequence, NO Parallel / Async execution here
            await InvokeFailureHandlersAsync(false, failureHandlers);

        protected async Task InvokeFailureHandlersAsync(bool executeInParallel, params Func<Task>[] failureHandlers)
        {
            // https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
            if (failureHandlers != null)
                if (executeInParallel)
                    Parallel.ForEach(failureHandlers, failureHandler => _exceptionHandler.AttachAndRun(failureHandler).Wait());
                else
                    await Task.WhenAll(failureHandlers.Select(failureHandler => _exceptionHandler.AttachAndRun(failureHandler)).ToArray());
        }

        protected IActionResult HandleException(Exception ex)
        {
            if (ex is ControllerException ctrlEx && !ctrlEx.IsCommandException && !ctrlEx.IsQueryException)
            {
                // Dont log for command ex, as they were already logged
                _exceptionHandler.HandleException(ex);
                return ctrlEx.ExceptionResult;
            }

            _exceptionHandler.HandleException(ex);
            return new NotOkResult(new ServiceResult(CopiousErrorCodes.Exception.AddInfo(ex.Message))); //if sensitive messages is paseed to client make sure you hide it / format.
        }

        public RequestContext Context => _contextProvider.Context;

        public void AddContext(Operation operation) => operation.SetContext(_contextProvider.Fn());

    }
}