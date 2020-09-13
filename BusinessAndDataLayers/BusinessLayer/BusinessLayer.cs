using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public class BusinessLayer : IBusinessLayer
    {
        #region Fields
        private readonly SaveAsync _saveAsync;
        private readonly WhereAsync _whereAsync;
        private readonly DeleteAsync _deleteAsync;
        private readonly Deleting _deleting;
        private readonly Deleted _deleted;
        private readonly Saving _saving;
        private readonly Saved _saved;
        private readonly BeforeGet _beforeGet;
        private readonly AfterGet _afterGet;
        #endregion

        #region Constructor
        public BusinessLayer(
            SaveAsync saveAsync = null,
            WhereAsync whereAsync = null,
            DeleteAsync deleteAsync = null,
            Deleting deleting = null,
            Deleted deleted = null,
            Saving saving = null,
            Saved saved = null,
            BeforeGet beforeGet = null,
            AfterGet afterGet = null
           )
        {
            _whereAsync = whereAsync;
            _deleteAsync = deleteAsync;
            _saveAsync = saveAsync;
            _deleting = deleting;
            _deleted = deleted;
            _saving = saving;
            _saved = saved;
            _beforeGet = beforeGet;
            _afterGet = afterGet;
        }
        #endregion

        #region Public Methods
        public async Task<int> DeleteAsync(Type type, Expression predicate)
        {
            if(_deleteAsync==null) throw new NotImplementedException("Delete not implemented");

            if (_deleting != null) await _deleting.Invoke(type, predicate);
            var deleteCount = await _deleteAsync(type, predicate);
            if (_deleted != null) await _deleted(type, deleteCount);
            return deleteCount;
        }

        public async Task<IAsyncEnumerable<object>> WhereAsync(Expression predicate)
        {
            if (_whereAsync == null) throw new NotImplementedException("Where not implemented");

            var type = predicate.Type.GenericTypeArguments[0];

            if (_beforeGet != null) await _beforeGet(type, predicate);

            var results = await _whereAsync(predicate);

            if (_afterGet != null) await _afterGet(type, results);

            return results;
        }

        public async Task<object> SaveAsync(object item, bool isUpdate)
        {
            if (_saveAsync == null) throw new NotImplementedException("Save not implemented");

            if (_saving != null) await _saving(item, isUpdate);
            var insertedItem = await _saveAsync(item, isUpdate);
            if (_saved != null) await _saved(insertedItem, isUpdate);

            return insertedItem;
        }
        #endregion
    }
}
