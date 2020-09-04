using Microsoft.Extensions.Logging;

namespace EntityFrameworkCoreGetSQL
{
    public class SingletonLoggerProvider : ILoggerProvider
    {
        #region Fields
        ILogger _logger;
        #endregion

        #region Constructor
        public SingletonLoggerProvider(ILogger logger)
        {
            _logger = logger;
        }
        #endregion

        #region Implementation
        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
        }
        #endregion
    }
}
