using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    /// <summary>
    /// This is a modified repository. It is not the standard DDD version of a repository
    /// Note: Transaction is left off these methods, but a transaction will probably need to be passed around so that database calls can access the transaction. It could be IDbTransaction, or a new interface like IDbTransaction
    /// </summary>
    public interface IRepository
    {
        Task<IAsyncEnumerable<object>> GetAsync(Type type, IQuery query);
        Task<object> InsertAsync(object item);
        Task<object> UpdateAsync(object item);
        Task DeleteAsync(Type type, Guid key);
    }
}
