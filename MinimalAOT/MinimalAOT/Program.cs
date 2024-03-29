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
        var sqlQuery = myDbContext.Set<Todo>().Where(x => true).ToQueryString();
        var query= myDbContext.Database.SqlQuery<Todo>(FormattableStringFactory.Create(sqlQuery));
        return await query.ToListAsync();
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
