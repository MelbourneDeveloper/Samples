using System.Collections.Immutable;

namespace GraphQLDynamic.Model
{
    public record AttributeDefinition
    (
        string Name,
        string Label,
        string HintText,
        string Category,
        string Heading,
        AttributeType AttributeType,
        ImmutableList<DropDownItemDefinition> DropDownItemDefinitions
    );
}
