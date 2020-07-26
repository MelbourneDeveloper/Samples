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

        public Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { typeof(T) });

            var dbSets = (IQueryable<T>)setMethodWithTypeArgument.Invoke(_dbContext, null);

            var whereMethod = typeof(Queryable).GetMethod(nameof(Queryable.Where), new Type[] { });

            return Task.FromResult(Queryable.Where(dbSets, predicate).ToAsyncEnumerable());
        }

        //Note this should be working
        public Task<IAsyncEnumerable<object>> GetAsync(Expression predicate)
        {
            //Get the entity type from the predicate
            var type = predicate.Type.GenericTypeArguments[0];

            //Get the set method
            var setMeth = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });

            var setMethodWithTypeArgument = setMeth.MakeGenericMethod(new Type[] { type });

            //Get the DbSet
            var dbSet = (IQueryable)setMethodWithTypeArgument.Invoke(_dbContext, null);

            //Warning extremely dodgy
            //TODO: Fix
            var whereMethod = typeof(Queryable).GetMethods().Where(m => m.Name == "Where").First().MakeGenericMethod(type);

            //Invoke where
            var result = whereMethod.Invoke(null, new object[] { dbSet, predicate });

            //Warning: More dodge here
            //Cast to enumerable of objects
            var castMethod = typeof(Queryable).GetMethods().Where(m => m.Name == "Cast").First().MakeGenericMethod(typeof(object));

            //Cast to IEnumerable<object>
            var enumerableObjects = (IEnumerable<object>)castMethod.Invoke(null, new object[] { result });

            //Conver to async and return
            return Task.FromResult(enumerableObjects.ToAsyncEnumerable());
        }
        

        public Task<object> SaveAsync(object item, bool isUpdate)
        {
            throw new NotImplementedException();
        }

    }
}
