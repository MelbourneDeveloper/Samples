using System.Collections.Immutable;

namespace GraphQLDynamic.Model
{
    public record TypeDefinition
    (
        ImmutableList<AttributeDefinition> AttributeDefinitions,
        ImmutableList<Relationship> Relationships
    );
}
