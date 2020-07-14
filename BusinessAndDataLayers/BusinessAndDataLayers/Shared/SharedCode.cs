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

    #region Classes
    public class DummyQuery : IQuery
    {
        //TODO
    }

    public class Person { public string Name { get; set; } }
    #endregion
}
