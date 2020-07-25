
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
            return Task.FromResult(_dbConnection.Query(predicate).ToAsyncEnumerable());
        }


        public Task<object> SaveAsync(object item, bool isUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
