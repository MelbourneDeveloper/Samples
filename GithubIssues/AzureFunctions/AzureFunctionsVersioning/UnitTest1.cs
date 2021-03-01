using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;

namespace AzureFunctionsVersioning
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateDbContext()
        {
            using var context = new Context();
        }

        [TestMethod]
        public void TestLogging()
        {
            Logger.Log();
        }
    }
}
