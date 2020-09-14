using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    //public delegate IServiceProvider BuildServiceProvider();

    public class BusinessLayer : IBusinessLayer
    {
        #region Fields
        private IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public BusinessLayer(
            IServiceProvider serviceProvider
           )
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Public Methods
        public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
        {
            await _serviceProvider.GetRequiredService<Deleting<T>>()(predicate);
            var deleteCount = await _serviceProvider.GetRequiredService<DeleteAsync<T>>()(predicate);
            await _serviceProvider.GetRequiredService<Deleted<T>>()(deleteCount);
            return deleteCount;
        }

        public async Task<IAsyncEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var beforeGet = _serviceProvider.GetService<BeforeGet<T>>();
            if (beforeGet != null) await beforeGet(predicate);
            var results = await _serviceProvider.GetRequiredService<WhereAsync<T>>()(predicate);
            var afterGet = _serviceProvider.GetService<AfterGet<T>>();
            if (afterGet != null) await afterGet(results);
            return results;
        }

        public async Task<T> SaveAsync<T>(T item, bool isUpdate)
        {
            var saving = _serviceProvider.GetService<Saving<T>>();
            var saved = _serviceProvider.GetService<Saved<T>>();
            if (saving != null) await saving(item, isUpdate);
            var saveAsync = _serviceProvider.GetRequiredService<SaveAsync<T>>();
            var insertedItem = await saveAsync(item, isUpdate);
            if (saved != null) await saved(insertedItem, isUpdate);
            return insertedItem;
        }
        #endregion
    }
}
