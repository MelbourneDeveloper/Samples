using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    /// <summary>
    /// This is a modified repository. It is not the standard DDD version of a repository
    /// Note: Transaction is left off these methods, but a transaction will probably need to be passed around so that database calls can access the transaction. It could be IDbTransaction, or a new interface like IDbTransaction
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets records
        /// </summary>
        /// <typeparam name="T">Not a fan of the reference type constraint here but Repo Db requires it</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<object> SaveAsync(object item, bool isUpdate);
        Task<int> DeleteAsync(Type type, object key);
    }
}
