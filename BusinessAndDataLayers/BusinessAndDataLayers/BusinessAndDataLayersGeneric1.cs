using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAndDataLayersGeneric1
{
    #region Interfaces
    /// <summary>
    /// This is a modified repository. It is not the standard DDD version of a repository
    /// Note: Transaction is left off these methods, but a transaction will probably need to be passed around so that database calls can access the transaction. It could be IDbTransaction, or a new interface like IDbTransaction
    /// </summary>
    public interface IRepository
    {
        Task<IAsyncEnumerable<object>> GetAsync(Type type, IQuery query);
        Task<object> InsertAsync(object item);
        Task<object> UpdateAsync(object item);
        Task DeleteAsync(Type type, Guid key);
    }

    /// <summary>
    /// Note IQuery could be an expresion, or have an expression as a member...
    /// Or, it could be replaced with IQueryable
    /// </summary>
    public interface IQuery
    {
    }

    public interface IExampleWrapper
    {
        Task<IAsyncEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> SavePersonAsync(Person person);
    }
    #endregion

    #region Delegates
    public delegate Task Deleting(Type type, Guid key);
    public delegate Task Deleted(Type type, Guid key);
    public delegate Task Inserting(object item);
    public delegate Task Inserted(object item);
    public delegate Task Updating(object item);
    public delegate Task Updated(object item);
    public delegate Task BeforeGet(Type type, IQuery query);
    public delegate Task AfterGet(IAsyncEnumerable<object> results);
    #endregion

    #region Extensions
    public static class RepositoryExtensions
    {
        public static async Task<T> SaveAsync<T>(this IRepository repository, T item)
        {
            //TODO: the query interface...
            var loadedItems = (await repository.GetAsync(typeof(T), new DummyQuery())).ToListAsync();

            if (loadedItems.Result.Count > 0)
            {
                return (T)await repository.UpdateAsync(item);
            }
            else
            {
                return (T)await repository.InsertAsync(item);
            }
        }

        public static async Task<IAsyncEnumerable<T>> GetAllAsync<T>(this IRepository repository)
        {
            //TODO: the query interface...
            return (await repository.GetAsync(typeof(T), new DummyQuery())).Cast<T>();
        }
    }
    #endregion

    #region Classes
    public class DummyQuery : IQuery
    {
        //TODO
    }

    public class BusinessLayer : IRepository
    {
        private IRepository _dataLayer;
        Deleting _deleting;
        Deleted _deleted;
        Inserting _inserting;
        Inserted _inserted;
        Updating _updating;
        Updated _updated;
        BeforeGet _beforeGet;
        AfterGet _afterGet;

        public BusinessLayer(
            IRepository dataLayer,
            Deleting deleting,
            Deleted deleted,
            Inserting inserting,
            Inserted inserted,
            Updating updating,
            Updated updated,
            BeforeGet beforeGet,
            AfterGet afterGet
           )
        {
            _dataLayer = dataLayer;
            _deleting = deleting;
            _deleted = deleted;
            _inserting = inserting;
            _inserted = inserted;
            _updating = updating;
            _updated = updated;
            _beforeGet = beforeGet;
            _afterGet = afterGet;
        }

        public async Task DeleteAsync(Type type, Guid key)
        {
            await _deleting(type, key);
            await _dataLayer.DeleteAsync(type, key);
            await _deleted(type, key);
        }

        public async Task<IAsyncEnumerable<object>> GetAsync(Type type, IQuery query)
        {
            await _beforeGet(type, query);
            var results = await _dataLayer.GetAsync(type, query);
            await _afterGet(results);
            return results;
        }

        public async Task<object> InsertAsync(object item)
        {
            await _inserting(item);
            var insertedItem = await _dataLayer.InsertAsync(item);
            await _inserted(insertedItem);
            return insertedItem;
        }

        public async Task<object> UpdateAsync(object item)
        {
            await _updating(item);
            var updatedItem = await _dataLayer.UpdateAsync(item);
            await _updated(item);
            return updatedItem;
        }
    }

    public class Person { public string Name { get; set; } }

    public class ExampleWrapper : IExampleWrapper
    {
        IRepository _businessLayer;

        public ExampleWrapper(IRepository businessLayer)
        {
            _businessLayer = businessLayer;
        }

        public Task<IAsyncEnumerable<Person>> GetAllPeopleAsync()
        {
            return _businessLayer.GetAllAsync<Person>();
        }

        public Task<Person> SavePersonAsync(Person person)
        {
            return _businessLayer.SaveAsync(person);
        }
    }

    public class ExampleApp
    {
        IExampleWrapper _exampleWrapper;

        public ExampleApp(IExampleWrapper exampleWrapper)
        {
            _exampleWrapper = exampleWrapper;
        }

        public Task CreateBobAsync()
        {
            return _exampleWrapper.SavePersonAsync(new Person { Name = "Bob" });
        }
    }
    #endregion
}
