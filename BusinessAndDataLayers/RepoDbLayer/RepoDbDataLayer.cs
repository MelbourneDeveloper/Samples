
using BusinessLayerLib;
using RepoDb;
using RepoDb.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepoDbLayer
{
    public class RepoDbDataLayer : IRepository
    {
        IDbConnection _dbConnection;

        public RepoDbDataLayer(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task DeleteAsync(Type type, Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Task.FromResult(_dbConnection.Query(predicate).ToAsyncEnumerable());
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
