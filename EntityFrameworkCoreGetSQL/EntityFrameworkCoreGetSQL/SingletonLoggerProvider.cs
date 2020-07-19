using Microsoft.Extensions.Logging;

namespace EntityFrameworkCoreGetSQL
{
    public class SingletonLoggerProvider : ILoggerProvider
    {
        ILogger _logger;

        public SingletonLoggerProvider(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
        }
    }
}
