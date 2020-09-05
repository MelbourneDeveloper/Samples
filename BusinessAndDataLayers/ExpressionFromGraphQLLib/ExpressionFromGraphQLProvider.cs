using EntityGraphQL.Compiler;
using EntityGraphQL.LinqQuery;
using EntityGraphQL.Schema;
using System.Linq.Expressions;

namespace ExpressionFromGraphQLLib
{
    public class ExpressionFromGraphQLProvider
    {
        private readonly ISchemaProvider _schemaProvider;

        public ExpressionFromGraphQLProvider(ISchemaProvider schemaProvider)
        {
            _schemaProvider = schemaProvider;
        }

        public Expression GetExpression(string graphQl)
        {
            var compiledQueryResult = EntityQueryCompiler.Compile(graphQl, _schemaProvider, null, new DefaultMethodProvider(), null);
            var expressionResult = compiledQueryResult.ExpressionResult;
            var whereMethodExpression = (dynamic)expressionResult.Expression;

            var secondArgument = whereMethodExpression.Arguments[1];

            //This is DB Context classes
            return (Expression)secondArgument.Operand;
        }
    }
}
