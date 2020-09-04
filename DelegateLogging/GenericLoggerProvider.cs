

using Microsoft.Extensions.Logging;
using System;

namespace DelegateLogging
{
    public class GenericLoggerProvider : ILoggerProvider
    {
        private Func<string, ILogger>? _createLogger;

        public GenericLoggerProvider(Func<string, ILogger> createLogger)
        {
            _createLogger = createLogger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger =  _createLogger?.Invoke(categoryName);

            if (logger != null) return logger;

            throw new InvalidOperationException();
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose() => _createLogger = null;
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }
}

