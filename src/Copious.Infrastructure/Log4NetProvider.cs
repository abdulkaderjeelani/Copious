using Microsoft.Extensions.Logging.Internal;
using System;
using System.Collections.Generic;
using log4net;
using Microsoft.Extensions.Logging;

namespace Copious.Infrastructure
{
    public class Log4NetProvider : ILoggerProvider
    {
        private IDictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public ILogger CreateLogger(string categoryName)
        {
            if (!_loggers.ContainsKey(categoryName))
            {
                lock (_loggers)
                {
                    // Have to check again since another thread may have gotten the lock first
                    if (!_loggers.ContainsKey(categoryName))
                    {
                        _loggers[categoryName] = new Log4NetAdapter(categoryName);
                    }
                }
            }
            return _loggers[categoryName];
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _loggers != null)
            {
                _loggers.Clear();
                _loggers = null;
            }
        }
    }

    public class Log4NetAdapter : ILogger
    {
        private readonly ILog _logger;

        public Log4NetAdapter(string loggerName)
        {
            _logger = LogManager.GetLogger("log4net-default-repository", loggerName);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;

                case LogLevel.Information:
                    return _logger.IsInfoEnabled;

                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;

                case LogLevel.Error:
                    return _logger.IsErrorEnabled;

                case LogLevel.Critical:
                    return _logger.IsFatalEnabled;

                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            string message;

            message = null != formatter ? formatter(state, exception) : state?.ToString() ?? string.Empty;

            message = $"Event: {eventId.Id} | {eventId.Name} " + message ?? string.Empty;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;

                case LogLevel.Information:
                    _logger.Info(message, exception);
                    break;

                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;

                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;

                case LogLevel.Critical:
                    _logger.Fatal(message, exception);
                    break;

                default:
                    _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    _logger.Info(message, exception);
                    break;
            }
        }
    }
}