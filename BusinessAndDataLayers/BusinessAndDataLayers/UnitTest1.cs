using BusinessAndDataLayers.Shared;
using BusinessAndDataLayersGeneric1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    [TestClass]
    public partial class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var mockDataLayer = new Mock<IRepository>();

            mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable()));

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<InsertingGeneric<Person>>(async (person) =>
            {
                person.Name += "Amended";
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dataLayer = new BusinessLayer(
                mockDataLayer.Object,
                async (a, b) => { },
                async (a, b) => { },
                async (entity) =>
                {
                    var insertingDelegateType = typeof(InsertingGeneric<>).MakeGenericType(new Type[] { entity.GetType() });

                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);

                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (a) => { },
                async (a) => { },
                async (a) => { },
                async (a, b) => { },
                async (a) => { });

            var person = await dataLayer.SaveAsync(new Person { Name = "Bob" });
        }
    }
}
