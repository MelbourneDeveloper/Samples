using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ILoggerSamples
{
    [TestClass]
    public class ILoggerTests
    {
        #region Fields
        private Mock<ILogger<LogTest>> _loggerMock;
        private LogTest _logTest;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<LogTest>>();
            _logTest = new LogTest(_loggerMock.Object);
        }
        #endregion

        #region Tests
        [TestMethod]
        public void TestLogInformation()
        {
            _logTest.Process();
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestLogError()
        {
            var recordId = new Guid("0b88ae00-7889-414a-aa26-18f206470001");

            _logTest.ProcessWithException(recordId);

            _loggerMock.Verify
            (
                l => l.Log
                (
                    //Check the severity level
                    LogLevel.Error,
                    //This may or may not be relevant to your scenario
                    It.IsAny<EventId>(),
                    //This is the magical Moq code that exposes internal log processing from the extension methods
                    It.Is<It.IsAnyType>((state, t) =>
                        //This confirms that the correct log message was sent to the logger. {OriginalFormat} should match the value passed to the logger
                        //Note: messages should be retrieved from a service that will probably store the strings in a resource file
                        CheckValue(state, LogTest.ErrorMessage, "{OriginalFormat}") &&
                        //This confirms that an argument with a key of "recordId" was sent with the correct value
                        //In Application Insights, this will turn up in Custom Dimensions
                        CheckValue(state, recordId, nameof(recordId))
                ),
                //Confirm the exception type
                It.IsAny<NotImplementedException>(),
                //Accept any valid Func here. The Func is specified by the extension methods
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                //Make sure the message was logged the correct number of times
                Times.Exactly(1)
            );
        }

        [TestMethod]
        public void TestConsoleLoggingWithBeginScope()
        {
            var hostBuilder = Host.CreateDefaultBuilder().
            ConfigureLogging((builderContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsole((options) =>
                {
                    //This displays arguments from the scope
                    options.IncludeScopes = true;
                });
            });

            var host = hostBuilder.Build();
            var logger = host.Services.GetRequiredService<ILogger<LogTest>>();

            //This specifies that every time a log message is logged, the correlation id will be logged as part of it
            using (logger.BeginScope("Correlation ID: {correlationID}", 123))
            {
                logger.LogInformation("Test");
                logger.LogInformation("Test2");
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Checks that a given key exists in the given collection, and that the value matches 
        /// </summary>
        private static bool CheckValue(object state, object expectedValue, string key)
        {
            var keyValuePairList = (IReadOnlyList<KeyValuePair<string, object>>)state;

            var actualValue = keyValuePairList.First(kvp => string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0).Value;

            return expectedValue.Equals(actualValue);
        }
        #endregion
    }
}
