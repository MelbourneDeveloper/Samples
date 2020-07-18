using BusinessAndDataLayers.Shared;
using BusinessLayerLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayersGeneric1
{
    #region Classes
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
    #endregion
}
