using Microsoft.Extensions.Logging;

namespace EntityFrameworkCoreGetSQL
{
    public class LoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger();
        }

        public void Dispose()
        {
        }
    }
}
