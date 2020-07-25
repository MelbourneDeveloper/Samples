
using BusinessLayerLib;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepoDbLayer
{
    public class RepoDbDataLayer
    {
        IDbConnection _dbConnection;

        public RepoDbDataLayer(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<int> DeleteAsync(Type type, object key)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncEnumerable<object>> GetAsync(Expression predicate)
        {
            Expression<Func<string, bool>> ass = null;
            _dbConnection.Query(ass);

            var method = typeof(DbConnectionExtension).GetMethod(nameof(DbConnectionExtension.Query));

 
            //public static IEnumerable<TEntity> Query<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> where = null, IEnumerable<OrderField> orderBy = null, int? top = 0, string hints = null, string cacheKey = null, int cacheItemExpiration = 180, int? commandTimeout = null, IDbTransaction transaction = null, ICache cache = null, ITrace trace = null, IStatementBuilder statementBuilder = null) where TEntity : class;


            return Task.FromResult(_dbConnection.Query(predicate).ToAsyncEnumerable());
        }


        public Task<object> SaveAsync(object item, bool isUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
