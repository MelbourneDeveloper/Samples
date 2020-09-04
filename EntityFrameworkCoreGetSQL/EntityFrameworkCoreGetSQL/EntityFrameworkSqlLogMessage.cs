using Microsoft.Extensions.Logging;
using System.Data;

namespace EntityFrameworkCoreGetSQL
{
    public class EntityFrameworkSqlLogMessage
    {
        public EntityFrameworkSqlLogMessage(
            EventId eventId,
            string commandText,
            string parameters,
            CommandType commandType,
            int commandTimeout,
            string elapsed
          )
        {
            EventId = eventId;
            CommandText = commandText;
            Parameters = parameters;
            CommandType = commandType;
            Elapsed = elapsed;
            CommandTimeout = commandTimeout;
        }

        public string Elapsed { get; }
        public int CommandTimeout { get; }
        public EventId EventId { get; }
        public string CommandText { get; }
        public string Parameters { get; }
        public CommandType CommandType { get; }
    }

}

