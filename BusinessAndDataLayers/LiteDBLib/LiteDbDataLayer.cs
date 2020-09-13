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
        private readonly LiteDatabase _db;

        public LiteDbDataLayer(LiteDatabase db)
        {
            _db = db;
        }

        public Task<IAsyncEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var liteCollection = _db.GetCollection<T>();
            var liteQueryable = liteCollection.Query();
            var list = liteQueryable.Where(predicate).ToList();
            return Task.FromResult(list.ToAsyncEnumerable());
        }
    }
}
