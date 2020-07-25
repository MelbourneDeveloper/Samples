using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class EntityFrameworkDataLayer 
    {
        DbContext _dbContext;

        public EntityFrameworkDataLayer(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> DeleteAsync(Type type, object key)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncEnumerable<object>> GetAsync(Expression predicate)
        {
            throw new NotImplementedException();
            /*
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { typeof(T) });

            var dbSets = (IQueryable<T>)setMethodWithTypeArgument.Invoke(_dbContext, null);

            var whereMethod = typeof(Queryable).GetMethod(nameof(Queryable.Where), new Type[] { });

            return Task.FromResult(Queryable.Where(dbSets, predicate).ToAsyncEnumerable());

            */
        }

        public Task<object> SaveAsync(object item, bool isUpdate)
        {
            throw new NotImplementedException();
        }

    }
}
