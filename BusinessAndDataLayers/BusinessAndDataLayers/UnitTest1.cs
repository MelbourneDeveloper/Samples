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
            //Arrange
            var person = new Person { Name = "Bob" };

            var mockDataLayer = new Mock<IRepository>();

            mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable()));
            mockDataLayer.Setup(r => r.UpdateAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(person));

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<InsertingGeneric<Person>>(async (p) =>
            {
                p.Name += "Insert";
            });

            serviceCollection.AddSingleton<UpdatingGeneric<Person>>(async (p) =>
            {
                p.Name += "Update";
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dataLayer = new BusinessLayer(
                mockDataLayer.Object,
                async (type, key) => { },
                async (type, key) => { },
                async (entity) =>
                {
                    var insertingDelegateType = typeof(InsertingGeneric<>).MakeGenericType(new Type[] { entity.GetType() });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (entity) => { },
                async (entity) =>
                {
                    var insertingDelegateType = typeof(UpdatingGeneric<>).MakeGenericType(new Type[] { entity.GetType() });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (entity) => { },
                async (type, query) => { },
                async (items) => { });

            //Act
            var savedPerson = await dataLayer.SaveAsync(person);

            //Assert
            Assert.AreEqual("BobUpdate", savedPerson.Name);
        }
    }
}
