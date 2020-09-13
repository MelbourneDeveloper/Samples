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

    public delegate Task Deleting(Type type, Expression predicate);
    public delegate Task Deleted(Type type, int count);
    public delegate Task Saving(object item, bool isUpdate);
    public delegate Task Saved(object item, bool isUpdate);
    public delegate Task BeforeGet(Type type, Expression predicate);
    public delegate Task AfterGet(Type type, object results);


    public delegate Task Deleting<T>(Expression predicate);
    public delegate Task Deleted<T>(int count);
    public delegate Task Saved<in T>(T item, bool isUpdate);
    public delegate Task AfterGet<in T>(IAsyncEnumerable<T> results);
    public delegate Task Saving<in T>(T item, bool isUpdate);
    public delegate Task BeforeGet<T>(Expression predicate);

}
