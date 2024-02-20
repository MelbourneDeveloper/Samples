namespace InsertGeneratorSample;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class SourceGenAttribute : Attribute
{
 
}

[SourceGen]
public partial class Class1
{

}
