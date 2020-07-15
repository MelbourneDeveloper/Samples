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
        BusinessLayer _businessLayer;
        Person _bob = new Person { Key = new Guid("087aca6b-61d4-4d94-8425-1bdfb34dab38"), Name = "Bob" };
        private bool _customDeleting = false;
        private bool _customDeleted = false;
        #endregion

        #region Tests
        [TestMethod]
        public async Task TestUpdating()
        {
            //Arrange

            //Return 1 person
            _mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(true)));

            //Act
            var savedPerson = await _businessLayer.SaveAsync(_bob);

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
            var savedPerson = await _businessLayer.SaveAsync(_bob);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobInsertingInserted", savedPerson.Name);

            //Verify insert was called
            _mockDataLayer.Verify(d => d.InsertAsync(It.IsAny<Person>()), Times.Once);
        }

        [TestMethod]
        public async Task TestDeleted()
        {
            //Arrange

            //Return no people
            _mockDataLayer.Setup(r => r.GetAsync(It.IsAny<Type>(), It.IsAny<IQuery>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(false)));

            //Act
            await _businessLayer.DeleteAsync<Person>(_bob.Key);

            //Verify insert was called
            _mockDataLayer.Verify(d => d.DeleteAsync(typeof(Person), _bob.Key), Times.Once);

            Assert.IsTrue(_customDeleted && _customDeleting);
        }
        #endregion

        #region Arrange
        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataLayer = new Mock<IRepository>();
            _mockDataLayer.Setup(r => r.UpdateAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(_bob));
            _mockDataLayer.Setup(r => r.InsertAsync(It.IsAny<object>())).Returns(Task.FromResult<object>(_bob));

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
            .AddSingleton<Deleting<Person>>(async (key) =>
            {
                _customDeleting = key == _bob.Key;
            })
            .AddSingleton<Deleted<Person>>(async (key) =>
            {
                _customDeleted = key == _bob.Key;
            })
             ;

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _businessLayer = new BusinessLayer(
                _mockDataLayer.Object,
                async (type, key) =>
                {
                    var insertingDelegateType = typeof(Deleting<>).MakeGenericType(new Type[] { type });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { key });
                },
                async (type, key) =>
                {
                    var insertingDelegateType = typeof(Deleted<>).MakeGenericType(new Type[] { type });
                    var insertingDelegate = (Delegate)serviceProvider.GetRequiredService(insertingDelegateType);
                    await (Task)insertingDelegate.DynamicInvoke(new object[] { key });
                }, async (entity) =>
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
