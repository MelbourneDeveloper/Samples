using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public class BusinessLayer
    {
        #region Fields
        private readonly SaveAsync _saveAsync;
        private readonly GetAsync _getAsync;
        private readonly DeleteAsync _deleteAsync;
        private readonly Deleting _deleting;
        private readonly Deleted _deleted;
        private readonly Saving _inserting;
        private readonly Inserted _inserted;
        private readonly BeforeGet _beforeGet;
        private readonly AfterGet _afterGet;
        #endregion

        #region Constructor
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
        #endregion

        #region Public Methods
        public async Task<int> DeleteAsync(Type type, object key)
        {
            await _deleting(type, key);
            var deleteCount = await _deleteAsync(type, key);
            await _deleted(type, key, deleteCount);
            return deleteCount;
        }

        public async Task<IAsyncEnumerable<object>> GetAsync(Expression predicate)
        {
            var type = predicate.Type.GenericTypeArguments[0];

            if (_beforeGet != null)
            {
                await _beforeGet(type, predicate);
            }

            var results = await _getAsync(predicate);

            if (_afterGet != null)
            {
                //Note: IAsyncEnumerable doesn't have a non-generic version
                await _afterGet(type, results);
            }

            return results;
        }

        public async Task<object> SaveAsync(object item, bool isUpdate)
        {
            await _inserting(item, isUpdate);
            var insertedItem = await _saveAsync(item, isUpdate);
            await _inserted(insertedItem, isUpdate);

            return insertedItem;
        }
        #endregion
    }
}
