using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EntityFrameworkCoreGetSQL
{
    public class EntityFrameworkSqlLogger : ILogger
    {
        Action<EntityFrameworkSqlLogMessage> _logMessage;

        public EntityFrameworkSqlLogger(Action<EntityFrameworkSqlLogMessage> logMessage)
        {
            _logMessage = logMessage;
        }

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
            if (state is IReadOnlyList<KeyValuePair<string, object>> keyValuePairList)
            {
                var entityFrameworkSqlLogMessage = new EntityFrameworkSqlLogMessage
                (
                    (string)keyValuePairList.FirstOrDefault(k => k.Key == "commandText").Value,
                    (string)keyValuePairList.FirstOrDefault(k => k.Key == "parameters").Value,
                    (CommandType)keyValuePairList.FirstOrDefault(k => k.Key == "commandType").Value
                );

                _logMessage(entityFrameworkSqlLogMessage);
            }
        }
    }

    public class EntityFrameworkSqlLogMessage
    {
        public EntityFrameworkSqlLogMessage(
            string commandText,
            string parameters,
            CommandType commandType
            )
        {
            CommandText = commandText;
            Parameters = parameters;
            CommandType = commandType;
        }

        public string CommandText { get; }
        public string Parameters { get; }
        public CommandType CommandType { get; }
    }

}

