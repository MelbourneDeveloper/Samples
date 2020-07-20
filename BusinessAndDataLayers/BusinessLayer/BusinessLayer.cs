using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IAsyncEnumerable<object>> GetAsync(Type type, IQueryable queryable)
        {
            await _beforeGet(type, queryable);
            var results = await _dataLayer.GetAsync(type, queryable);
            await _afterGet(type, results);
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
}
