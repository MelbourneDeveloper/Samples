using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MinimalAOT;
using MinimalAOT.Models;

var builder = WebApplication.CreateSlimBuilder(args);
builder
    .Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.TypeInfoResolverChain.Insert(
            0,
            MinimalAOT.AppJsonSerializerContext.Default
        );
    })
    .AddLogging(b => b.AddConsole())
    .AddDbContext<MyDbContext>(
        options =>
            options.UseSqlite("Data Source=mydatabase.db").UseModel(MyDbContextModel.Instance)
    );

builder.Logging.AddConsole();

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();

var groupBuilder = app.MapGroup("/todos");
groupBuilder.MapGet(
    "/",
    async (MyDbContext myDbContext) =>
    {
        var query = myDbContext.Set<Todo>().Where(x => x.Id == 1);
        var sqlQuery = query.ToQueryString();
        var asdasd= myDbContext.Database.SqlQuery<Todo>(FormattableStringFactory.Create(sqlQuery));
        var results = await asdasd.ToListAsync();
        return results;
    }
);
groupBuilder.MapPost(
    "/",
    async (Todo todo, MyDbContext myDbContext) =>
    {
        try
        {
            //myDbContext.Todos.Add(todo);
            //await myDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
);

app.Run();
