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
        #region Fields
        Mock<IRepository> _mockDataLayer;
        BusinessLayer _dataLayer;
        Person person = new Person { Name = "Bob" };
        #endregion

        #region Tests
        [TestMethod]
        public async Task TestMethod1()
        {
            //Act
            var savedPerson = await _dataLayer.SaveAsync(person);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobUpdate", savedPerson.Name);

            //Verify update was called
            _mockDataLayer.Verify(d => d.UpdateAsync(It.IsAny<Person>()), Times.Once);
        }
        #endregion

        #region Arrange
        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataLayer = new Mock<IRepository>();
            _mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable()));
            _mockDataLayer.Setup(r => r.UpdateAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(person));

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<InsertingGeneric<Person>>(async (p) =>
            {
                p.Name += "Insert";
            })
            .AddSingleton<UpdatingGeneric<Person>>(async (p) =>
            {
                p.Name += "Update";
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _dataLayer = new BusinessLayer(
                _mockDataLayer.Object,
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
        }
        #endregion
    }
}
