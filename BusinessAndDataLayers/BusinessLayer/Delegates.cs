using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerLib
{
    public delegate Task Deleting(Type type, Guid key);
    public delegate Task Deleted(Type type, Guid key);
    public delegate Task Inserting(object item);
    public delegate Task Inserted(object item);
    public delegate Task Updating(object item);
    public delegate Task Updated(object item);
    public delegate Task BeforeGet(Type type, IQuery query);
    public delegate Task AfterGet(Type type, IAsyncEnumerable<object> results);


    public delegate Task Deleting<T>(Guid key);
    public delegate Task Deleted<T>(Guid key);
    public delegate Task Inserted<T>(T item);
    public delegate Task Updated<T>(T item);
    public delegate Task AfterGet<T>(IAsyncEnumerable<object> results);
    public delegate Task Inserting<T>(T item);
    public delegate Task Updating<T>(T item);
    public delegate Task BeforeGet<T>(IQuery query);

}
