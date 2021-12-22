using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQLDynamic.Model;
using Jayse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Immutable;

#pragma warning disable CA2201 // Do not raise reserved exception types


namespace GraphQLDynamic
{
    [TestClass]
    public class GraphQLTests
    {

        [TestMethod]
        public async Task TestDynamicSchema2()
        {
            var expectedAsset = new TestAsset(1, "R2-D2", new Location(1000, 1200));
            var assetJson = JsonConvert.SerializeObject(expectedAsset);
            var jsonObject = assetJson.ToJsonObject();

            if (jsonObject == null) throw new Exception();

            var locationType = new TypeDefinition(
            ImmutableList.Create
            (
                new AttributeDefinition[]
                {
                    new AttributeDefinition("Latitude", "Latitude", "Latitude", "Location", "Location", AttributeType.Number, ImmutableList<DropDownItemDefinition>.Empty),
                    new AttributeDefinition("Longitude", "Longitude", "Longitude", "Location", "Location", AttributeType.Number, ImmutableList<DropDownItemDefinition>.Empty)
                }),
                ImmutableList<Relationship>.Empty
            );

            var assetType = new TypeDefinition(
            ImmutableList.Create
            (
                new AttributeDefinition[]
                {
                    new AttributeDefinition("Id", "Id", "Id", "Details", "Main Details", AttributeType.Number, ImmutableList<DropDownItemDefinition>.Empty),
                    new AttributeDefinition("Name", "Name", "Name", "Details", "Main Details", AttributeType.Text, ImmutableList<DropDownItemDefinition>.Empty)
                }),
                ImmutableList.Create(new Relationship("Location", locationType, RelationshipCategory.OneToOne))
            );

            using var schema = assetType.ToDynamicObjectGraphSchema();

            var json = await schema.ExecuteAsync(_ =>
            {
                _.Query = "{ id name  }";
                _.Root = jsonObject;
            }).ConfigureAwait(false);

            var actalDroid = JsonConvert.DeserializeObject<Root<TestAsset>>(json);

            if (actalDroid?.Data == null) throw new Exception();

            Assert.AreEqual(expectedAsset.Id, actalDroid.Data.Id);
            Assert.AreEqual(expectedAsset.Name, actalDroid.Data.Name);
        }
    }

    public record TestAsset(int Id, string Name, Location Location);

    public record Location(double Latitude, double Longitude);

    public class Root<T>
    {
        public T? Data { get; set; }
    }


}

