﻿using GraphQL.Types;
using GraphQLDynamic.Model;
using Jayse;

public class DynamicObjectGraphType : ObjectGraphType
{
    public DynamicObjectGraphType(TypeDefinition typeDefinition)
        => Process(this, typeDefinition ?? throw new ArgumentNullException(nameof(typeDefinition)));

    private static void Process(
        ComplexGraphType<object> graphType,
        TypeDefinition typeDefinition)
    {
        foreach (var attributeDefinition in typeDefinition.AttributeDefinitions)
        {
            Process(graphType, attributeDefinition.Name, attributeDefinition.AttributeType);
        }

        /*
        foreach (var relationship in typeDefinition.Relationships)
        {
            if (relationship.RelationshipCategory == RelationshipCategory.OneToOne)
            {
                var fieldBuilder =
                    graphType.Field<ObjectGraphType>()
                    .Name(relationship.Name)
                    .Returns<object>()
                    .Resolve(context =>
                        ((OrderedImmutableDictionary<string, JsonValue>)context.Source!)[relationship.Name].ObjectValue
                        );

                //The graph type here should recurse deeper but I can't figure out what to do here...
                Process(graphType, relationship.RelationshipType);
            }
            else
            {
                var fieldBuilder =
                graphType.Field<ListGraphType>()
                .Name(relationship.Name)
                .Returns<object>()
                .Resolve(context =>
                    ((OrderedImmutableDictionary<string, JsonValue>)context.Source!)[relationship.Name].ObjectValue
                    );

                //The graph type here should recurse deeper but I can't figure out what to do here...
                Process(graphType, relationship.RelationshipType);
            }
        }
        */
    }

    private static void Process(
        ComplexGraphType<object> graphType,
        string attributeName,
        AttributeType attributeType)
    {
        object _ = attributeType switch
        {
            AttributeType.Number => graphType.Field<IntGraphType>()
                                     .Name(attributeName)
                                     .Returns<int>()
                                     .Resolve(context =>
                                        (int)((OrderedImmutableDictionary<string, JsonValue>)context.Source!)[attributeName].NumberValue
                                     ),

            AttributeType.Text => graphType.Field<StringGraphType>()
                                     .Name(attributeName)
                                     .Returns<string>()
                                     .Resolve(context =>
                                        ((OrderedImmutableDictionary<string, JsonValue>)context.Source!)[attributeName].StringValue
                                     ),

            AttributeType.Date => throw new NotImplementedException(),
            AttributeType.Time => throw new NotImplementedException(),
            AttributeType.DateTime => throw new NotImplementedException(),
            AttributeType.DropDown => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }
}

