using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public interface IBusinessLayer
    {
        Task<int> DeleteAsync(Type type, Expression predicate);
        Task<IAsyncEnumerable<object>> WhereAsync(Expression predicate);
        Task<object> SaveAsync(object item, bool isUpdate);
    }
}