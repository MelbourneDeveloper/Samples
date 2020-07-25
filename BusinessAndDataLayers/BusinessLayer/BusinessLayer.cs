using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public class BusinessLayer
    {
        private SaveAsync _saveAsync;
        private GetAsync _getAsync;
        private DeleteAsync _deleteAsync;
        Deleting _deleting;
        Deleted _deleted;
        Saving _inserting;
        Inserted _inserted;
        BeforeGet _beforeGet;
        AfterGet _afterGet;

        public BusinessLayer(
            SaveAsync saveAsync = null,
            GetAsync getAsync = null,
            DeleteAsync deleteAsync = null,
            Deleting deleting = null,
            Deleted deleted = null,
            Saving inserting = null,
            Inserted inserted = null,
            BeforeGet beforeGet = null,
            AfterGet afterGet = null
           )
        {
            _getAsync = getAsync;
            _deleteAsync = deleteAsync;
            _saveAsync = saveAsync;
            _deleting = deleting;
            _deleted = deleted;
            _inserting = inserting;
            _inserted = inserted;
            _beforeGet = beforeGet;
            _afterGet = afterGet;
        }

        public async Task<int> DeleteAsync(Type type, object key)
        {
            await _deleting(type, key);
            var deleteCount = await _deleteAsync(type, key);
            await _deleted(type, key, deleteCount);
            return deleteCount;
        }

        public async Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            await _beforeGet(typeof(T), predicate);
            var results = await _getAsync.GetAsync(predicate);
            //Note: IAsyncEnumerable doesn't have a non-generic version
            await _afterGet(typeof(T), results);
            return results;
        }

        public async Task<object> SaveAsync(object item, bool isUpdate)
        {
            await _inserting(item, isUpdate);
            var insertedItem = await _saveAsync(item, isUpdate);
            await _inserted(insertedItem, isUpdate);

            return insertedItem;
        }
    }
}
