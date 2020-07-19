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
        public void TestMethod1()
        {



            using (var ordersDbContext = new OrdersDbContext(new LoggerProvider()))
            {
                var orderLines = ordersDbContext.OrderLines.Where(o => o.Id == Guid.Empty).ToList();
                orderLines = ordersDbContext.OrderLines.ToList();
            }
        }
    }
}
