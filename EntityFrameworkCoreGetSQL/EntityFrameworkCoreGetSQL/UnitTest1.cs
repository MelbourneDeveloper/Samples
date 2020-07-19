using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EntityFrameworkCoreGetSQL
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var entityFrameworkSqlLogger = new EntityFrameworkSqlLogger((m) => 
            { 
                Console.WriteLine(m.CommandText); 
            });

            using (var ordersDbContext = new OrdersDbContext(new SingletonLoggerProvider(entityFrameworkSqlLogger)))
            {
                var orderLines = ordersDbContext.OrderLines.Where(o => o.Id == Guid.Empty).ToList();
                orderLines = ordersDbContext.OrderLines.ToList();
            }
        }
    }
}
