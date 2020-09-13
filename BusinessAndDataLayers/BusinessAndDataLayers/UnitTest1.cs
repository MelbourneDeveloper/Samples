using BusinessLayerLib;
using DomainLib;
using EntityGraphQL.Schema;
using ExpressionFromGraphQLLib;
using LiteDB;
using LiteDBLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepoDb;
using RepoDbLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFDataLayer;
using RepoDb.Enumerations;
using RepoDb.Extensions;
using System.Reflection;
using OrmLiteDbLayer;

namespace BusinessAndDataLayers
{
    [TestClass]
    public partial class UnitTest1
    {
        private const string LiteDbFileName = "MyData.db";
        private const string CustomValue = "Custom";
        #region Fields

        private Mock<WhereAsync> _mockGet;
        private Mock<SaveAsync> _mockSave;
        private Mock<DeleteAsync> _mockDelete;
        private BusinessLayer _businessLayer;
        private readonly Person _bob = new Person { Key = new Guid("087aca6b-61d4-4d94-8425-1bdfb34dab38"), Name = "Bob" };
        private readonly string _id = Guid.NewGuid().ToString().Replace("-", "*");
        private bool _customDeleting;
        private bool _customDeleted;
        private bool _customBefore;
        private bool _customAfter;
        private Expression _getOrderByIdPredicate;
        #endregion

        #region Tests

        [TestMethod]
        public async Task TestGetEntityFramework()
        {
            await CreateOrdersDb();

            await using var ordersDbContext = new OrdersDbContext();
            var entityFrameworkDataLayer = new EntityFrameworkDataLayer(ordersDbContext);
            var asyncEnumerable = await entityFrameworkDataLayer.WhereAsync((Expression<Func<OrderRecord, bool>>)_getOrderByIdPredicate);
            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }


        [TestMethod]
        public async Task TestGetEntityFrameworkViaGraphQL()
        {
            var schema = SchemaBuilder.FromObject<OrdersDbContext>();

            var expressionFromGraphQLProvider = new ExpressionFromGraphQLProvider(schema);

            var expression = expressionFromGraphQLProvider.GetExpression($@"orderRecord.where(id = ""{_id}"")");

            await CreateOrdersDb();

            await using var ordersDbContext = new OrdersDbContext();
            var entityFrameworkDataLayer = new EntityFrameworkDataLayer(ordersDbContext);
            var asyncEnumerable = await entityFrameworkDataLayer.WhereAsync((Expression<Func<OrderRecord, bool>>)expression);
            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }



        [TestMethod]
        public async Task TestGetDbLiteViaGraphQL()
        {
            using var db = SetupLiteDb();
            var schema = SchemaBuilder.FromObject<OrdersDbContext>();

            var expressionFromGraphQLProvider = new ExpressionFromGraphQLProvider(schema);

            var expression = expressionFromGraphQLProvider.GetExpression($@"orderRecord.where(id = ""{_id}"")");

            await CreateOrdersDb();

            var repoDbDataLayer = new LiteDbDataLayer(db);
            var asyncEnumerable = await repoDbDataLayer
                .GetAsync((Expression<Func<OrderRecord, bool>>)expression);

            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }

        //[TestMethod]
        //public async Task TestGetRepoDbViaGraphQL()
        //{
        //    await CreateOrdersDb();

        //    var schema = SchemaBuilder.FromObject<OrdersDbContext>();

        //    var expressionFromGraphQLProvider = new ExpressionFromGraphQLProvider(schema);

        //    var expression = expressionFromGraphQLProvider.GetExpression($@"orderRecord.where(id = ""{_id}"")");

        //    await CreateOrdersDb();

        //    using var connection = new SQLiteConnection(OrdersDbContext.ConnectionString);
        //    var repoDbDataLayer = new RepoDbDataLayer(connection);
        //    var asyncEnumerable = await repoDbDataLayer
        //        .WhereAsync((Expression<Func<OrderRecord, bool>>)expression);

        //    var returnValue = await asyncEnumerable.ToListAsync();
        //    Assert.AreEqual(1, returnValue.Count);
        //}

        [TestMethod]
        public async Task TestGetRepoDb()
        {
            await CreateOrdersDb();

            SqLiteBootstrap.Initialize();

            await using var connection = new SQLiteConnection(OrdersDbContext.ConnectionString);
            var repoDbDataLayer = new RepoDbDataLayer(connection);
            var asyncEnumerable = await repoDbDataLayer.WhereAsync((Expression<Func<OrderRecord, bool>>)_getOrderByIdPredicate);


            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }

        [TestMethod]
        public async Task TestGetOrmLite()
        {
            await CreateOrdersDb();

            var repoDbDataLayer = new OrmLiteLayer("Orders.db");
            var asyncEnumerable = await repoDbDataLayer.WhereAsync((Expression<Func<OrderRecord, bool>>)_getOrderByIdPredicate);

            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }

        [TestMethod]
        public async Task TestOrmLiteViaGraphQL()
        {
            var schema = SchemaBuilder.FromObject<OrdersDbContext>();

            var expressionFromGraphQLProvider = new ExpressionFromGraphQLProvider(schema);

            var expression = expressionFromGraphQLProvider.GetExpression($@"orderRecord.where(id = ""{_id}"")");

            await CreateOrdersDb();

            var repoDbDataLayer = new OrmLiteLayer("Orders.db");
            var asyncEnumerable = await repoDbDataLayer.WhereAsync((Expression<Func<OrderRecord, bool>>)expression);
            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }

        [TestMethod]
        public async Task TestOrmLiteViaGraphQLBusinessRules()
        {
            //Configure Expressions to GraphQL
            var schema = SchemaBuilder.FromObject<OrdersDbContext>();
            var expressionFromGraphQLProvider = new ExpressionFromGraphQLProvider(schema);

            //Get an expression from graphql
            var expression = expressionFromGraphQLProvider.GetExpression($@"orderRecord.where(id = ""{_id}"")");

            //Create the SQLite DB
            await CreateOrdersDb();

            //Create the data layer
            var ormLiteLayer = new OrmLiteLayer("Orders.db");

            //Create a business layer and rules
            var businessLayer = GetBusinessRulesContainer(ormLiteLayer);

            //Add a rule
            businessLayer.Item1.OnFetched<OrderRecord>(async (a) =>
            {
                var results = await a.ToListAsync();
                results.First().CustomValue = CustomValue;
            });

            //Load the records
            var asyncEnumerable = await businessLayer.Item2.WhereAsync(expression);
            var returnValue = await asyncEnumerable.ToListAsync();

            //Check that the business rule was called
            var resultOrder = (OrderRecord)returnValue.First();
            Assert.AreEqual(CustomValue, resultOrder.CustomValue);
        }

        private static (ServiceCollection, BusinessLayer) GetBusinessRulesContainer(OrmLiteLayer ormLiteLayer)
        {
            var serviceCollection = new ServiceCollection();


            var businessLayer = new BusinessLayer(
                null,
                ormLiteLayer.WhereAsync,
                null,
                null,
                null,
                null,
                null,
                async (type, query) =>
                {
                    var serviceProvider = serviceCollection.BuildServiceProvider();
                    var delegateType = typeof(BeforeGet<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(query);
                },
                async (type, items) =>
                {
                    var serviceProvider = serviceCollection.BuildServiceProvider();
                    var delegateType = typeof(AfterGet<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(items);
                }
                );
            return (serviceCollection, businessLayer);
        }

        [TestMethod]
        public async Task TestGetLiteDb()
        {
            using var db = SetupLiteDb();
            var repoDbDataLayer = new LiteDbDataLayer(db);
            var asyncEnumerable = await repoDbDataLayer
                .GetAsync((Expression<Func<OrderRecord, bool>>)_getOrderByIdPredicate);

            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
        }

        [TestMethod]
        public async Task TestGetLiteDbWithBusinessLayer()
        {
            using var db = SetupLiteDb();
            var liteDbDataLayer = new LiteDbDataLayer(db);

            var businessLayer = new BusinessLayer(whereAsync: liteDbDataLayer.GetAsync);

            var getAsync = (WhereAsync)businessLayer.WhereAsync;

            var asyncEnumerable = getAsync.GetAsync((Expression<Func<OrderRecord, bool>>)_getOrderByIdPredicate);

            var returnValue = (await asyncEnumerable).ToListAsync().Result;
            Assert.AreEqual(1, returnValue.Count);
        }

        [TestMethod]
        public async Task TestGetWithBusinessLayer()
        {
            await CreateOrdersDb();

            SqLiteBootstrap.Initialize();

            await using var connection = new SQLiteConnection(OrdersDbContext.ConnectionString);
            var repoDbDataLayer = new RepoDbDataLayer(connection);

            var businessLayer = new BusinessLayer(
                whereAsync: repoDbDataLayer.WhereAsync,
                beforeGet: (t, e) =>
                {
                    _customBefore = true;
                    return Task.FromResult(true);
                },
                afterGet: (t, result) =>
                {
                    _customAfter = true;
                    return Task.FromResult(true);
                });

            WhereAsync whereAsync = businessLayer.WhereAsync;

            var asyncEnumerable = await whereAsync
                .GetAsync<OrderRecord>(o => o.Id == _id);


            var returnValue = await asyncEnumerable.ToListAsync();
            Assert.AreEqual(1, returnValue.Count);
            Assert.IsTrue(_customBefore && _customAfter);
        }


        [TestMethod]
        public async Task TestUpdating()
        {
            //Arrange

            //Return 1 person
            _mockGet.Setup(r => r(It.IsAny<Expression<Func<Person, bool>>>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(true)));

            //Act
            var savedPerson = (Person)await _businessLayer.SaveAsync(_bob, true);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobUpdatingUpdated", savedPerson.Name);

            //Verify update was called
            _mockSave.Verify(d => d(It.IsAny<Person>(), true), Times.Once);
        }

        [TestMethod]
        public async Task TestInserted()
        {
            //Act
            var savedPerson = (Person)await _businessLayer.SaveAsync(_bob, false);

            //Assert

            //Verify custom business logic
            Assert.AreEqual("BobInsertingInserted", savedPerson.Name);

            //Verify insert was called
            _mockSave.Verify(d => d(It.IsAny<Person>(), false), Times.Once);
        }

        [TestMethod]
        public async Task TestDeleted()
        {

            //Act
            var expression = _mockGet.Object.CreateQueryExpression<Person>(p => p.Key == _bob.Key);
            _mockDelete.Setup(d => d(typeof(Person), expression)).Returns(Task.FromResult(1));
            await _businessLayer.DeleteAsync(typeof(Person), expression);

            //Verify insert was called
            _mockDelete.Verify(d => d(typeof(Person), expression), Times.Once);

            Assert.IsTrue(_customDeleted && _customDeleting);
        }

        [TestMethod]
        public async Task TestGet()
        {
            //Act
            WhereAsync whereAsync = _businessLayer.WhereAsync;
            var people = await whereAsync.GetAsync<Person>(p => true);

            //Verify get was called
            _mockGet.Verify(d => d(It.IsAny<Expression<Func<Person, bool>>>()), Times.Once);

            Assert.IsTrue(_customBefore && _customAfter);

            //This returns an empty list by default
            Assert.AreEqual(0, (await people.ToListAsync()).Count);
        }


        #endregion
        #region Arrange
        [TestInitialize]
        public void TestInitialize()
        {
            _mockGet = new Mock<WhereAsync>();
            _mockSave = new Mock<SaveAsync>();
            _mockDelete = new Mock<DeleteAsync>();

            _mockSave.Setup(r => r(It.IsAny<object>(), true)).Returns(Task.FromResult<object>(_bob));
            _mockSave.Setup(r => r(It.IsAny<object>(), false)).Returns(Task.FromResult<object>(_bob));
            _mockGet.Setup(r => r(It.IsAny<Expression<Func<Person, bool>>>())).Returns(Task.FromResult<IAsyncEnumerable<object>>(new DummyPersonAsObjectAsyncEnumerable(false)));

            _getOrderByIdPredicate = _mockGet.Object.CreateQueryExpression<OrderRecord>(o => o.Id == _id);
            _businessLayer = GetBusinessLayer().businessLayer;
        }

        private async Task CreateOrdersDb()
        {
            await using var ordersDbContext = new OrdersDbContext();
            await ordersDbContext.OrderRecord.AddAsync(new OrderRecord { Id = _id });
            await ordersDbContext.SaveChangesAsync();
        }

        private LiteDatabase SetupLiteDb()
        {
            if (File.Exists(LiteDbFileName)) File.Delete(LiteDbFileName);
            var db = new LiteDatabase(LiteDbFileName);

            var orders = db.GetCollection<OrderRecord>();
            orders.Insert(new OrderRecord { Id = _id, Name = "123" });

            return db;
        }

        private (BusinessLayer businessLayer, ServiceProvider serviceProvider) GetBusinessLayer()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureBusinessRules(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var businessLayer = new BusinessLayer(
                _mockSave.Object,
                _mockGet.Object,
                _mockDelete.Object,
                async (type, key) =>
                {
                    var delegateType = typeof(Deleting<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(key);
                },
                async (type, count) =>
                {
                    var delegateType = typeof(Deleted<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(new object[] { count });
                }, async (entity, isUpdate) =>
                {
                    var delegateType = typeof(Saving<>).MakeGenericType(entity.GetType());
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(entity, isUpdate);
                },
                async (entity, isUpdate) =>
                {
                    var delegateType = typeof(Saved<>).MakeGenericType(entity.GetType());
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(entity, isUpdate);
                },
                async (type, query) =>
                {
                    var delegateType = typeof(BeforeGet<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(query);
                },
                async (type, items) =>
                {
                    var delegateType = typeof(AfterGet<>).MakeGenericType(type);
                    var @delegate = (Delegate)serviceProvider.GetService(delegateType);
                    if (@delegate == null) return;
                    await (Task)@delegate.DynamicInvoke(items);
                }
                );

            return (businessLayer, serviceProvider);
        }

        private void ConfigureBusinessRules(IServiceCollection serviceCollection)
        {
            serviceCollection.OnSaving<Person>((p, u) =>
            {
                if (u)
                {
                    p.Name += "Updating";
                }
                else
                {
                    p.Name += "Inserting";
                }

                return Task.FromResult(true);
            })
            .OnSaved<Person>((p, u) =>
            {
                if (u)
                {
                    p.Name += "Updated";
                }
                else
                {
                    p.Name += "Inserted";
                }
                return Task.FromResult(true);
            })
            .OnDeleting<Person>(e =>
            {
                var expression = (Expression<Func<Person, bool>>)e;
                var body = (dynamic)expression.Body;
                var left = body.Left;
                var leftMember = left.Member;
                var getMethods = (MethodInfo)leftMember.GetMethod;
                var bobKey = (Guid)getMethods.Invoke(_bob, null);
                _customDeleting = _bob.Key == bobKey;
                return Task.FromResult(true);
            })
            .OnDeleted<Person>(count =>
            {
                _customDeleted = count == 1;
                return Task.FromResult(true);
            })
            .OnFetching<Person>(e =>
            {
                _customBefore = true;
                return Task.FromResult(true);
            })
            .OnFetched<Person>(people =>
            {
                _customAfter = true;
                return Task.FromResult(true);
            });
        }
        #endregion
    }
}
