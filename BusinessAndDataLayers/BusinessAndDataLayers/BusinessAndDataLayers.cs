using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
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

    #region Extensions
    public static class RepositoryExtensions
    {
        public static async Task<T> SaveAsync<T>(this RepositoryBase<T> repository, T item)
        {
            //TODO: the query interface...
            var loadedItems = (await repository.GetAsync(new DummyQuery())).ToListAsync();

            if (loadedItems.Result.Count > 0)
            {
                return await repository.UpdateAsync(item);
            }
            else
            {
                return await repository.InsertAsync(item);
            }
        }

        public static async Task<IAsyncEnumerable<T>> GetAllAsync<T>(this RepositoryBase<T> repository)
        {
            //TODO: the query interface...
            return await repository.GetAsync(new DummyQuery());
        }
    }
    #endregion

    #region Classes
    /// <summary>
    /// This is a modified repository. It is not the standard DDD version of a repository
    /// Note: Transaction is left off these methods, but a transaction will probably need to be passed around so that database calls can access the transaction. It could be IDbTransaction, or a new interface like IDbTransaction
    /// </summary>
    public abstract class RepositoryBase<T>
    {
        public abstract Task<IAsyncEnumerable<T>> GetAsync(IQuery query);
        public abstract Task<T> InsertAsync(T item);
        public abstract Task<T> UpdateAsync(T item);
        public abstract Task DeleteAsync(Guid key);
    }

    public class DummyQuery : IQuery
    {
        //TODO
    }



    public class PersonBusinessLayer
    {
        RepositoryBase<Person> _dataLayer;

        public async Task DeleteAsync(Guid key)
        {
            //Deleting business logic
            await _dataLayer.DeleteAsync(key);
            //Deleted business logic
        }

        public Task<IAsyncEnumerable<Person>> GetAsync(IQuery query)
        {
            return _dataLayer.GetAsync(query);
        }

        public async Task<Person> InsertAsync(Person item)
        {
            //inserting business logic
            var person = await _dataLayer.InsertAsync(item);
            //inserted business logic

            return person;
        }

        public async Task<object> UpdateAsync(Person item)
        {
            //updating business logic
            var person = await _dataLayer.UpdateAsync(item);
            //updated business logic

            return person;
        }
    }

    public class Person { public string Name { get; set; } }

    /*
       public class ExampleWrapper : IExampleWrapper
       {
           IRepository _businessLayer;

           public ExampleWrapper(IRepository businessLayer)
           {
               _businessLayer = businessLayer;
           }

           public Task<IAsyncEnumerable<Person>> GetAllPeopleAsync()
           {
               return _businessLayer.GetAllAsync<Person>();
           }

           public Task<Person> SavePersonAsync(Person person)
           {
               return _businessLayer.SaveAsync(person);
           }
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


       */

    #endregion
}
