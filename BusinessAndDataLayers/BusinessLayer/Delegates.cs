using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    #region CRUD
    public delegate Task<IAsyncEnumerable<object>> WhereAsync(Expression predicate);
    public delegate Task<object> SaveAsync(object item, bool isUpdate);
    public delegate Task<int> DeleteAsync(Type type, object key);
    #endregion


    internal delegate Expression GetExpression(string graphQl);

    public delegate Task Deleting(Type type, object key);
    public delegate Task Deleted(Type type, object key, int count);
    public delegate Task Saving(object item, bool isUpdate);
    public delegate Task Inserted(object item, bool isUpdate);
    public delegate Task BeforeGet(Type type, Expression predicate);
    public delegate Task AfterGet(Type type, object results);


    public delegate Task Deleting<T>(Guid key);
    public delegate Task Deleted<T>(Guid key);
    public delegate Task Saved<T>(T item, bool isUpdate);
    public delegate Task AfterGet<T>(IAsyncEnumerable<T> results);
    public delegate Task Saving<T>(T item, bool isUpdate);
    public delegate Task BeforeGet<T>(Expression predicate);

}
