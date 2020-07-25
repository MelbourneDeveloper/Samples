using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LiteDBLib
{
    public class LiteDbDataLayer
    {
        LiteDatabase _db;

        public LiteDbDataLayer(LiteDatabase db)
        {
            _db = db;
        }

        public Task<int> DeleteAsync(Type type, object key)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncEnumerable<object>> GetAsync(Expression predicate)
        {
            throw new NotImplementedException();
            /*
            // Get a collection (or create, if doesn't exist)
            var orders = _db.GetCollection<T>("OrderRecords");

            // Use LINQ to query documents (filter, sort, transform)
            var results = orders.Query()
                .Where(predicate)
                .ToList();

            return Task.FromResult(results.ToAsyncEnumerable());

            */
        }


        public Task<object> SaveAsync(object item, bool isUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
