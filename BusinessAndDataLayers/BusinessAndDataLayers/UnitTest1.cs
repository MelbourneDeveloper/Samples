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
        Person _person = new Person { Name = "Bob" };
        #endregion

        #region Tests
        [TestMethod]
        public async Task TestUpdating()
        {
            //Arrange

            //Return 1 person
            _mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(true)));

            //Act
            var savedPerson = await _dataLayer.SaveAsync(_person);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobUpdatingUpdated", savedPerson.Name);

            //Verify update was called
            _mockDataLayer.Verify(d => d.UpdateAsync(It.IsAny<Person>()), Times.Once);
        }

        [TestMethod]
        public async Task TestInserted()
        {
            //Arrange

            //Return no people
            _mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(false)));

            //Act
            var savedPerson = await _dataLayer.SaveAsync(_person);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobInsertingInserted", savedPerson.Name);

            //Verify insert was called
            _mockDataLayer.Verify(d => d.InsertAsync(It.IsAny<Person>()), Times.Once);
        }
        #endregion

        #region Arrange
        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataLayer = new Mock<IRepository>();
            _mockDataLayer.Setup(r => r.UpdateAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(_person));
            _mockDataLayer.Setup(r => r.InsertAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(_person));

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<InsertingGeneric<Person>>(async (p) =>
            {
                p.Name += "Inserting";
            })
            .AddSingleton<UpdatingGeneric<Person>>(async (p) =>
            {
                p.Name += "Updating";
            })
            .AddSingleton<UpdatedGeneric<Person>>(async (p) =>
            {
                p.Name += "Updated";
            })
            .AddSingleton<InsertedGeneric<Person>>(async (p) =>
            {
                p.Name += "Inserted";
            })
             ;

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
                async (entity) =>
                {
                    var insertingDelegateType = typeof(InsertedGeneric<>).MakeGenericType(new Type[] { entity.GetType() });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (entity) =>
                {
                    var insertingDelegateType = typeof(UpdatingGeneric<>).MakeGenericType(new Type[] { entity.GetType() });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (entity) =>
                {
                    var insertingDelegateType = typeof(UpdatedGeneric<>).MakeGenericType(new Type[] { entity.GetType() });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { entity });
                },
                async (type, query) => { },
                async (items) => { });
        }
        #endregion
    }
}
