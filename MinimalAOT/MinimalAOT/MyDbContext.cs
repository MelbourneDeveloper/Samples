using Microsoft.EntityFrameworkCore;

class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite("Data Source=mydatabase.db")
                .UseModel(YourProjectNamespace.MyDbContextModel.Instance);
        }
    }

    public DbSet<Todo> Todos { get; }
}