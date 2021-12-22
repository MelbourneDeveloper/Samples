using GraphQLDynamic.Model;

namespace GraphQLDynamic
{
    public static class DynamicObjectGraphSchemaExtensions
    {
        public static DynamicObjectGraphSchema ToDynamicObjectGraphSchema(this TypeDefinition typeDefinition)
            => new(typeDefinition);
    }

}
