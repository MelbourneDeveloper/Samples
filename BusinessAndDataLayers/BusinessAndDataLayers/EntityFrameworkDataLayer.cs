using BusinessLayerLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class EntityFrameworkDataLayer : IBusinessLayer
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

        public Task<IAsyncEnumerable<object>> GetAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { typeof(T) });

            var dbSets = setMethodWithTypeArgument.Invoke(_dbContext, null);


            return null;
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
