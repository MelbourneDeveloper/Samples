namespace InsertUpdateGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

[Generator]
public class SqlInsertSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver that will be created for each compilation
        context.RegisterForSyntaxNotifications(() => new ModelSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Retrieve the populated receiver 
        if (context.SyntaxReceiver is not ModelSyntaxReceiver receiver)
            return;

        foreach (var classModel in receiver.CandidateClasses)
        {
            var source = GenerateInsertStatement(classModel);
            context.AddSource($"{classModel.Identifier.Text}_InsertStatement.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private string GenerateInsertStatement(ClassDeclarationSyntax classModel)
    {
        var className = classModel.Identifier.ValueText;
        var properties = classModel.Members.OfType<PropertyDeclarationSyntax>();

        var columns = new StringBuilder();
        var values = new StringBuilder();

        foreach (var property in properties)
        {
            var propName = property.Identifier.ValueText;
            columns.Append($"{propName}, ");
            values.Append($"@{propName}, ");
        }

        columns.Length -= 2; // Remove last comma
        values.Length -= 2; // Remove last comma

        return $@"
public partial class {className}
{{
    public string GetInsertStatement()
    {{
        return ""INSERT INTO {className} ({columns}) VALUES ({values});"";
    }}
}}
";
    }

    class ModelSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Look for classes with a specific attribute or naming convention
            if (syntaxNode is ClassDeclarationSyntax classDeclaration &&
                classDeclaration.AttributeLists.Count > 0)
            {
                CandidateClasses.Add(classDeclaration);
            }
        }
    }
}
