using System;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace Copious.Infrastructure
{
    public class ExceptionHandler : IExceptionHandler
    {
        readonly ILogger _logger;

        public ExceptionHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExceptionHandler>();
        }

        public void HandleException(Exception ex)
        {
            _logger.LogCritical(new EventId(CopiousErrorCodes.ExceptionCode, ex.Message), ex, ex.Message);
        }
    }
}