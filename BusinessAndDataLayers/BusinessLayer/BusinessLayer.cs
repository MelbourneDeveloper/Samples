using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public class BusinessLayer : IRepository
    {
        private IRepository _dataLayer;
        Deleting _deleting;
        Deleted _deleted;
        Inserting _inserting;
        Inserted _inserted;
        BeforeGet _beforeGet;
        AfterGet _afterGet;

        public BusinessLayer(
            IRepository dataLayer,
            Deleting deleting = null,
            Deleted deleted = null,
            Inserting inserting = null,
            Inserted inserted = null,
            BeforeGet beforeGet = null,
            AfterGet afterGet = null
           )
        {
            _dataLayer = dataLayer;
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
            var deleteCount = await _dataLayer.DeleteAsync(type, key);
            await _deleted(type, key, deleteCount);
            return deleteCount;
        }

        public async Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            await _beforeGet(typeof(T), predicate);
            var results = await _dataLayer.GetAsync(predicate);
            //Note: IAsyncEnumerable doesn't have a non-generic version
            await _afterGet(typeof(T), results);
            return results;
        }

        public async Task<object> SaveAsync(object item, bool isUpdate)
        {
            await _inserting(item, isUpdate);
            var insertedItem = await _dataLayer.SaveAsync(item, isUpdate);
            await _inserted(insertedItem, isUpdate);

            return insertedItem;
        }
    }
}
