using BusinessAndDataLayers.Shared;
using BusinessLayerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    #region Interfaces
    /// <summary>
    /// This is a modified repository. It is not the standard DDD version of a repository
    /// Note: Transaction is left off these methods, but a transaction will probably need to be passed around so that database calls can access the transaction. It could be IDbTransaction, or a new interface like IDbTransaction
    /// </summary>
    public interface IRepository<T>
    {
        Task DeleteAsync(Guid key);
        Task<IAsyncEnumerable<T>> GetAsync(IQuery query);
        Task<T> InsertAsync(T item);
        Task<T> UpdateAsync(T item);
    }
    #endregion

    #region Extensions
    public static class RepositoryExtensions
    {
        public static async Task<T> SaveAsync<T>(this IRepository<T> repository, T item)
        {
            //TODO: the query interface...
            var loadedItems = (await repository.GetAsync(null)).ToListAsync();

            if (loadedItems.Result.Count > 0)
            {
                return await repository.UpdateAsync(item);
            }
            else
            {
                return await repository.InsertAsync(item);
            }
        }

        public static async Task<IAsyncEnumerable<T>> GetAllAsync<T>(this IRepository<T> repository)
        {
            //TODO: the query interface...
            return await repository.GetAsync(null);
        }
    }
    #endregion

    #region Classes
    /// <summary>
    /// ***We need one of these per entity to implement the business logic. This guarantees that we need code generation***
    /// Note: this doesn't need to implement IRepository<Person> but it can. This might confuse the IoC container so it may be better to create a separate interface...
    /// </summary>
    public class PersonBusinessLayer : IRepository<Person>
    {
        //Note we may want to create an interface as IPersonRepository in scenarios where the interface needs methods specific for Person

        IRepository<Person> _dataLayer;

        public PersonBusinessLayer(IRepository<Person> dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public async Task DeleteAsync(Guid key)
        {
            //The comments here represent code that would be filled in. We would probably create a partial class and call the custom code in these parts from the generated code.

            //But, all this just begs the question, why not just put delegates here as per the other examples?

            //TODO: Deleting business logic
            await _dataLayer.DeleteAsync(key);
            //TODO: Deleted business logic
        }

        public async Task<IAsyncEnumerable<Person>> GetAsync(IQuery query)
        {
            //TODO: Before get logic
            var results = await _dataLayer.GetAsync(query);
            //TODO: After get logic
            return results;
        }

        public async Task<Person> InsertAsync(Person item)
        {
            //TODO: inserting business logic
            var person = await _dataLayer.InsertAsync(item);
            //TODO: inserted business logic

            return person;
        }

        public async Task<Person> UpdateAsync(Person item)
        {
            //TODO: updating business logic
            var person = await _dataLayer.UpdateAsync(item);
            //TODO: updated business logic
            return person;
        }
    }

    public class ExampleWrapper : IExampleWrapper
    {
        IRepository<Person> _businessLayer;

        public ExampleWrapper(IRepository<Person> businessLayer)
        {
            _businessLayer = businessLayer;
        }

        public Task<IAsyncEnumerable<Person>> GetAllPeopleAsync()
        {
            return _businessLayer.GetAsync(null);
        }

        public Task<Person> SavePersonAsync(Person person)
        {
            //This only works with the abstract class
            return _businessLayer.SaveAsync(person);
        }
    }
    #endregion
}
