//using System.Threading.Tasks;
//using BusinessLayerLib;
//using DomainLib;

//namespace BusinessAndDataLayers
//{
//    public class DemoController
//    {
//        private readonly WhereAsync _whereAsync;

//        public DemoController(WhereAsync whereAsync)
//        {
//            _whereAsync = whereAsync;
//        }

//        public Task GetAsync() => _whereAsync.GetAsync<OrderRecord>(o => o.Id == "123");

//    }
//}