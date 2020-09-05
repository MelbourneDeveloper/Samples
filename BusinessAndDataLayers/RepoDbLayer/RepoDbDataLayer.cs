using RepoDb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepoDbLayer
{
    public class RepoDbDataLayer
    {
        private readonly IDbConnection _dbConnection;

        public RepoDbDataLayer(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<IAsyncEnumerable<object>> WhereAsync(Expression predicate)
        {
            var type = predicate.Type.GenericTypeArguments[0];

            //Warning: Extremely dodgy hack!
            //TODO: Find a better way to do this and cache the method
            var methods = typeof(DbConnectionExtension).GetMethods().Where(m => m.Name == nameof(DbConnectionExtension.Query)).ToList();
            var methodInfo = methods.FirstOrDefault(m => m.GetParameters()[1].ParameterType.Name.Contains("Expression"));

            var genericMethod = methodInfo.MakeGenericMethod(type);

            //TODO: Casting here will degrade performance
            var result = (IList)genericMethod.Invoke(
                null,
                new object[]
                {
                    _dbConnection,
                    predicate,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
             });
            var list = new List<object>((IEnumerable<object>)result);

            return Task.FromResult(list.ToAsyncEnumerable());
        }
    }
}
