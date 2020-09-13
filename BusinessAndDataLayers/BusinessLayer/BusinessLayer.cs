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
            await _serviceProvider.GetRequiredService<BeforeGet<T>>()(predicate);
            var results = await _serviceProvider.GetRequiredService<WhereAsync<T>>()(predicate);
            await _serviceProvider.GetRequiredService<AfterGet<T>>()(results);
            return results;
        }

        public async Task<T> SaveAsync<T>(T item, bool isUpdate)
        {
            var saving = _serviceProvider.GetService<Saving<T>>();
            var saved = _serviceProvider.GetService<Saved<T>>();
            if (saving != null) await saving(item, isUpdate);
            var insertedItem = await _serviceProvider.GetRequiredService<SaveAsync<T>>()(item, isUpdate);
            if (saved != null) await saved(insertedItem, isUpdate);
            return insertedItem;
        }
        #endregion
    }
}
