using BusinessLayerLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class EntityFrameworkDataLayer : IRepository
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

        public Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { typeof(T) });

            var dbSets = (IQueryable<T>)setMethodWithTypeArgument.Invoke(_dbContext, null);

            var whereMethod = typeof(Queryable).GetMethod(nameof(Queryable.Where), new Type[] { });

            return Task.FromResult(Queryable.Where(dbSets, predicate).ToAsyncEnumerable());
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
