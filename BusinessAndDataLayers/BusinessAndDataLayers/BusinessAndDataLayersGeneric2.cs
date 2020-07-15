using BusinessAndDataLayers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAndDataLayersGeneric2
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
    public class DataLayer : IRepository
    {
        Deleting _deleting;
        Deleted _deleted;
        Inserting _inserting;
        Inserted _inserted;
        Updating _updating;
        Updated _updated;
        BeforeGet _beforeGet;
        AfterGet _afterGet;

        public DataLayer(
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
            //TODO: Implement data layer
            await _deleted(type, key);
        }

        public async Task<IAsyncEnumerable<object>> GetAsync(Type type, IQuery query)
        {
            await _beforeGet(type, query);
            //TODO: Implement data layer
            IAsyncEnumerable<object> results = null;
            await _afterGet(type, results);
            return results;
        }

        public async Task<object> InsertAsync(object item)
        {
            await _inserting(item);
            //TODO: Implement data layer
            object insertedItem = null;
            await _inserted(insertedItem);
            return insertedItem;
        }

        public async Task<object> UpdateAsync(object item)
        {
            await _updating(item);
            //TODO: Implement data layer
            object updatedItem = null;
            await _updated(item);
            return updatedItem;
        }
    }

    public class ExampleWrapper : IExampleWrapper
    {
        IRepository _dataLayer;

        public ExampleWrapper(IRepository dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public Task<IAsyncEnumerable<Person>> GetAllPeopleAsync()
        {
            return _dataLayer.GetAllAsync<Person>();
        }

        public Task<Person> SavePersonAsync(Person person)
        {
            return _dataLayer.SaveAsync(person);
        }
    }
    #endregion
}
