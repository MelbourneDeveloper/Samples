using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EntityFrameworkCoreGetSQL
{
    public class EntityFrameworkSqlLogger : ILogger
    {
        #region Fields
        Action<EntityFrameworkSqlLogMessage> _logMessage;
        #endregion

        #region Constructor
        public EntityFrameworkSqlLogger(Action<EntityFrameworkSqlLogMessage> logMessage)
        {
            _logMessage = logMessage;
        }
        #endregion

        #region Implementation
        public IDisposable BeginScope<TState>(TState state)
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (eventId.Id != 20101)
            {
                //Filter messages that aren't relevant.
                //There may be other types of messages that are relevant for other database platforms...

                return;
            }

            if (state is IReadOnlyList<KeyValuePair<string, object>> keyValuePairList)
            {
                var entityFrameworkSqlLogMessage = new EntityFrameworkSqlLogMessage
                (
                    eventId,
                    (string)keyValuePairList.FirstOrDefault(k => k.Key == "commandText").Value,
                    (string)keyValuePairList.FirstOrDefault(k => k.Key == "parameters").Value,
                    (CommandType)keyValuePairList.FirstOrDefault(k => k.Key == "commandType").Value,
                    (int)keyValuePairList.FirstOrDefault(k => k.Key == "commandTimeout").Value,
                    (string)keyValuePairList.FirstOrDefault(k => k.Key == "elapsed").Value
              );

                _logMessage(entityFrameworkSqlLogMessage);
            }
        }
        #endregion
    }

}

