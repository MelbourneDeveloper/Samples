using BusinessLayerLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayers.Shared
{
    #region Interfaces
    public interface IExampleWrapper
    {
        Task<IAsyncEnumerable<Person>> GetAllPeopleAsync();
        Task<Person> SavePersonAsync(Person person);
    }
    #endregion

    #region Classes
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
