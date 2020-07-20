using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EntityFrameworkCoreGetSQL
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LogWithCustomLogger()
        {
            var entityFrameworkSqlLogger = new EntityFrameworkSqlLogger((m) =>
            {
                Console.WriteLine($"SQL Query:\r\n{m.CommandText}\r\nElapsed:{m.Elapsed} millisecods\r\n\r\n");
            });

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
            });

            loggerFactory.AddProvider(new SingletonLoggerProvider(entityFrameworkSqlLogger));

            using (var ordersDbContext = new OrdersDbContext(loggerFactory))
            {
                var orderLines = ordersDbContext.OrderLines.Where(o => o.Id == Guid.Empty).ToList();
                orderLines = ordersDbContext.OrderLines.ToList();
            }
        }

        [TestMethod]
        public void LogWithConsoleLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddConsole((options) => { })
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
            });

            using (var ordersDbContext = new OrdersDbContext(loggerFactory))
            {
                IQueryable<OrderLine> queryables = ordersDbContext.OrderLines.Where(o => o.Id == Guid.Empty);

                var asdasd = queryables.ToQueryString();

                var orderLines = queryables.ToList();
                orderLines = ordersDbContext.OrderLines.ToList();
            }
        }
    }
}
