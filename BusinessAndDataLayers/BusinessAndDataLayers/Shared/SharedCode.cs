using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayers.Shared
{
    #region Interfaces
    /// <summary>
    /// Note IQuery could be an expresion, or have an expression as a member...
    /// Or, it could be replaced with IQueryable
    /// </summary>
    public interface IQuery
    {
    }

    public interface IExampleWrapper
    {
        Task<IAsyncEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> SavePersonAsync(Person person);
    }
    #endregion

    #region Delegates
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
    #endregion

    #region Classes
    public class DummyQuery : IQuery
    {
        //TODO
    }

    public class Person
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
    }

    public class ExampleApp
    {
        IExampleWrapper _exampleWrapper;

        public ExampleApp(IExampleWrapper exampleWrapper)
        {
            _exampleWrapper = exampleWrapper;
        }

        public Task CreateBobAsync()
        {
            return _exampleWrapper.SavePersonAsync(new Person { Name = "Bob" });
        }
    }
    #endregion
}
