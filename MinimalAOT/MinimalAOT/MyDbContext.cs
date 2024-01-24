using Microsoft.EntityFrameworkCore;
using MinimalAOT;
using YourProjectNamespace;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite("Data Source=mydatabase.db")
                .UseModel(MyDbContextModel.Instance); // Use your compiled model here
        }
    }

    public DbSet<Todo> Todos { get; set; }
}