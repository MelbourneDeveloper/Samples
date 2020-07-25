using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    #region Extensions
    public static class RepositoryExtensions
    {
        public static async Task<T> SaveAsync<T>(this IRepository repository, T item, bool isUpdate) where T : class
        {
            return (T)await repository.SaveAsync(item, isUpdate);
        }

        public static async Task<IAsyncEnumerable<T>> GetAllAsync<T>(this IRepository repository) where T : class
        {
            //TODO: the query interface...
            var asyncEnumerable = await repository.GetAsync<T>(null);

            return asyncEnumerable;
        }

        public static Task DeleteAsync<T>(this IRepository repository, Guid key)
        {
            //TODO: the query interface...
            return repository.DeleteAsync(typeof(T), key);
        }
    }
    #endregion
}
