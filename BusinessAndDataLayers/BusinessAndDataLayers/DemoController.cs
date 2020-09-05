using System.Threading.Tasks;
using BusinessLayerLib;

namespace BusinessAndDataLayers
{
    public class DemoController
    {
        private readonly WhereAsync _whereAsync;

        public DemoController(WhereAsync whereAsync)
        {
            _whereAsync = whereAsync;
        }

        public async Task GetAsync()
        {
            var orderRecords = _whereAsync.GetAsync<OrderRecord>(o => o.Id == "123");
        }
    }
}