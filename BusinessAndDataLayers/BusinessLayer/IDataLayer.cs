using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public interface IDataLayer
    {
        Task<IAsyncEnumerable<object>> GetAsync(Type type, string selectStatement, object[] parameters);
        Task<object> InsertAsync(object item);
        Task<object> UpdateAsync(object item);
        Task DeleteAsync(Type type, Guid key);
    }
}
