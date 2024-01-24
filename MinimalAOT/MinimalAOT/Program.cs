using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddLogging(b => b.AddConsole());
builder.Logging.AddConsole();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<MyDbContext>(
    options =>
        options
            .UseSqlite("Data Source=mydatabase.db")
            .UseModel(YourProjectNamespace.MyDbContextModel.Instance)
);

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", async (MyDbContext myDbContext) => await myDbContext.Todos.ToListAsync());

todosApi.MapPost(
    "/",
    async (Todo todo, MyDbContext myDbContext) =>
    {
        try
        {
            myDbContext.Todos.Add(todo);
            await myDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
);

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;