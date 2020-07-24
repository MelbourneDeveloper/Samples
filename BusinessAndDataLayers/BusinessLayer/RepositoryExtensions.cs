using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    #region Extensions
    public static class RepositoryExtensions
    {
        public static async Task<T> SaveAsync<T>(this IBusinessLayer repository, T item)
        {
            //TODO: the query interface...
            var loadedItems = (await repository.GetAsync<T>(null)).ToListAsync();

            if (loadedItems.Result.Count > 0)
            {
                return (T)await repository.UpdateAsync(item);
            }
            else
            {
                return (T)await repository.InsertAsync(item);
            }
        }

        public static async Task<IAsyncEnumerable<T>> GetAllAsync<T>(this IBusinessLayer repository)
        {
            //TODO: the query interface...
            var asyncEnumerable = await repository.GetAsync<T>(null);

            return asyncEnumerable;
        }

        public static Task DeleteAsync<T>(this IBusinessLayer repository, Guid key)
        {
            //TODO: the query interface...
            return repository.DeleteAsync(typeof(T), key);
        }
    }
    #endregion
}
