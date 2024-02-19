using System.Text.Json.Serialization;

namespace MinimalAOT;

[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(List<Todo>[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
