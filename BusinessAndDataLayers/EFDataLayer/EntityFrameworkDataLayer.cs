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

        //Note this should be working
        public Task<IAsyncEnumerable<object>> WhereAsync(Expression predicate)
        {
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
            var result = whereMethod.Invoke(null, new object[] { dbSet, predicate });

            //Warning: More dodge here
            //Cast to enumerable of objects
            var castMethod = typeof(Queryable).GetMethods().First(m => m.Name == nameof(Queryable.Cast)).MakeGenericMethod(typeof(object));

            //Cast to IEnumerable<object>
            var enumerableObjects = (IEnumerable<object>)castMethod.Invoke(null, new[] { result });

            //Convert to async and return
            return Task.FromResult(enumerableObjects.ToAsyncEnumerable());
        }
    }
}
