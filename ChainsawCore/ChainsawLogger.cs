using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace ChainsawCore
{
    public class ChainsawLogger : ILogger
    {
        private readonly IChainsawLoggerProvider loggerProvider;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly string categoryName;
        
        public ChainsawLogger(
            IChainsawLoggerProvider loggerProvider,
            IHostingEnvironment hostingEnvironment,
            string categoryName)
        {
            this.loggerProvider = loggerProvider;
            this.hostingEnvironment = hostingEnvironment;
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        // Write a log message
        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel,
            EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var logMessage = new LogMessage
            {
                Level = logLevel,
                Message = formatter.Invoke(state, exception),
                Thread = Thread.CurrentThread.ManagedThreadId,
            };
            logMessage.ApplicationName = this.hostingEnvironment.ApplicationName;
            logMessage.MachineName = Environment.MachineName;
            this.loggerProvider.WriteMessage(logMessage);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
        }

        public async Task LogInformationAsync(string message)
        {
            var logMessage = new LogMessage
            {
                Level = LogLevel.Information,
                Message = message,
                Thread = Thread.CurrentThread.ManagedThreadId,
            };
            logMessage.ApplicationName = this.hostingEnvironment.ApplicationName;
            logMessage.MachineName = Environment.MachineName;
            await this.loggerProvider.WriteMessageAsync(logMessage);
        }
    }
}