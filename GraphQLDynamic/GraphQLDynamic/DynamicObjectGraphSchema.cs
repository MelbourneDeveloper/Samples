using GraphQL.Types;
using GraphQLDynamic.Model;

namespace GraphQLDynamic
{
    public class DynamicObjectGraphSchema : Schema
    {
        public DynamicObjectGraphSchema(TypeDefinition typeDefinition)
            => Query = new DynamicObjectGraphType(typeDefinition);
    }

}
