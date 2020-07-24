using BusinessLayerLib;
using EntityFrameworkCoreGetSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class EntityFrameworkDataLayer : IDataLayer
    {
        DbContext _dbContext;

        public EntityFrameworkDataLayer(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DeleteAsync(Type type, Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncEnumerable<object>> GetAsync(Type type, string selectStatement, object[] parameters)
        {
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { type });

            var dbSets = (DbSet<Order>)setMethodWithTypeArgument.Invoke(_dbContext, null);

            var fromSqlRawMethod = typeof(RelationalQueryableExtensions).GetMethod(nameof(RelationalQueryableExtensions.FromSqlRaw)).MakeGenericMethod(new Type[] { type });

            var queryableOrders = fromSqlRawMethod.Invoke(null, new object[] { dbSets, selectStatement, new object[] {  } });

            var toAsyncEnumerableMethod = typeof(AsyncEnumerable).GetMethod(
                nameof(AsyncEnumerable.ToAsyncEnumerable),
                new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { type }) });

            var castMethod = typeof(AsyncEnumerable).GetMethod(nameof(AsyncEnumerable.Cast)).MakeGenericMethod(new Type[] { typeof(object) });

            var result = (IAsyncEnumerable<object>)castMethod.Invoke(null, new object[] { queryableOrders });

            return Task.FromResult(result);
        }

        public Task<object> InsertAsync(object item)
        {
            throw new NotImplementedException();
        }

        public Task<object> UpdateAsync(object item)
        {
            throw new NotImplementedException();
        }
    }
}
