using Microsoft.Extensions.Logging;
using System;

namespace ILoggerSamples
{
    /// <summary>
    /// A sample for using and testing ILogger 
    /// </summary>
    public class LogTest
    {
        private readonly ILogger _logger;
        public const string InformationMessage = "Test message";
        public const string ErrorMessage = "Not implemented {recordId}";

        public LogTest(ILogger<LogTest> logger)
        {
            _logger = logger;
        }

        public void Process()
        {
            _logger.LogInformation(InformationMessage);
        }

        public void ProcessWithException(Guid recordId)
        {
            try
            {
                throw new NotImplementedException(ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, recordId);
            }
        }
    }
}
