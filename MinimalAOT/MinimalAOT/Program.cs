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


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite("Data Source=mydatabase.db"));

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();


app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", async (a) =>
{
    var myDbContext = new MyDbContext(new DbContextOptions<MyDbContext>());
    await myDbContext.Todos.ToListAsync();
});
todosApi.MapPost("/", async (Todo todo) =>
{
    var myDbContext = new MyDbContext(new DbContextOptions<MyDbContext>());
    myDbContext.Todos.Add(todo);
    await myDbContext.SaveChangesAsync();
});


app.Run();


class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=mydatabase.db")
                .UseModel(YourProjectNamespace.MyDbContextModel.Instance);
        }
    }

    public DbSet<Todo> Todos { get; set; }
}

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}


public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation($"Handling request: {context.Request.Method} {context.Request.Path}");
        try
        {
            await _next(context); // Call the next delegate/middleware in the pipeline
        }
        finally
        {
            _logger.LogInformation($"Finished handling request. Status Code: {context.Response.StatusCode}");
        }
    }
}