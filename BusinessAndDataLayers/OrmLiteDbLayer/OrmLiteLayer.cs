using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLiteDbLayer
{
    public class OrmLiteLayer
    {
        IDbConnection _dbConnection;

        public OrmLiteLayer(string connectionString)
        {
            _dbConnection = new OrmLiteConnectionFactory(connectionString, SqliteDialect.Provider).CreateDbConnection();
            _dbConnection.Open();
        }

        public async Task<IAsyncEnumerable<T>> WhereAsync<T>(Expression<Func<T, bool>> predicate) => _dbConnection.Select(predicate).ToAsyncEnumerable();
    }
}
