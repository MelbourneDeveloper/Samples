namespace GraphQLDynamic.Model
{
    public record Relationship
    (
        string Name,
        TypeDefinition RelationshipType,
        RelationshipCategory RelationshipCategory
    );
}
