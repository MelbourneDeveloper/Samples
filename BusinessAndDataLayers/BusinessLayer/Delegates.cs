using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    delegate Expression GetExpression(string graphQl);

    public delegate Task Deleting(Type type, Guid key);
    public delegate Task Deleted(Type type, Guid key);
    public delegate Task Inserting(object item);
    public delegate Task Inserted(object item);
    public delegate Task Updating(object item);
    public delegate Task Updated(object item);
    public delegate Task BeforeGet(Type type, Expression predicate);
    public delegate Task AfterGet(Type type, object results);


    public delegate Task Deleting<T>(Guid key);
    public delegate Task Deleted<T>(Guid key);
    public delegate Task Inserted<T>(T item);
    public delegate Task Updated<T>(T item);
    public delegate Task AfterGet<T>(IAsyncEnumerable<T> results);
    public delegate Task Inserting<T>(T item);
    public delegate Task Updating<T>(T item);
    public delegate Task BeforeGet<T>(IQueryable queryable);

}
