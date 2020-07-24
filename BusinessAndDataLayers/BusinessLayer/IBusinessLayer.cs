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
    public interface IBusinessLayer
    {
        Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate);
        Task<object> InsertAsync(object item);
        Task<object> UpdateAsync(object item);
        Task DeleteAsync(Type type, Guid key);
    }
}
