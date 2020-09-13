using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFDataLayer
{
    public class EntityFrameworkDataLayer
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkDataLayer(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IAsyncEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate)
        {
            //NOTE: all this reflection is because EF doesn't allow value types. We can work around that like this

            //Get the entity type from the predicate
            var type = predicate.Type.GenericTypeArguments[0];

            //Get the set method
            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMethod.MakeGenericMethod(type);

            //Get the DbSet
            var dbSet = (IQueryable)setMethodWithTypeArgument.Invoke(_dbContext, null);

            //Warning extremely dodgy
            //TODO: Fix
            var whereMethod = typeof(Queryable).GetMethods().First(m => m.Name == nameof(Queryable.Where)).MakeGenericMethod(type);

            //Invoke where
            var result = (IQueryable<T>)whereMethod.Invoke(null, new object[] { dbSet, predicate });

            //Convert to async and return
            return Task.FromResult(result.ToAsyncEnumerable());
        }
    }
}
