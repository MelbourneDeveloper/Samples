using Microsoft.Extensions.Logging;

namespace Shared
{
    public class Logger
    {
        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => _ = builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

        public static void Log()
        {
            loggerFactory.CreateLogger("Hi").LogInformation("Hi");
        }

    }
}
