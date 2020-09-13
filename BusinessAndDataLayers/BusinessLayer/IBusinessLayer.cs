using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public interface IBusinessLayer
    {
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate);
        Task<IAsyncEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate);
        Task<T> SaveAsync<T>(T item, bool isUpdate);
    }
}