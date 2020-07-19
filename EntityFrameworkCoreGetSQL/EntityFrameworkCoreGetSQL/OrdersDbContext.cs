
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCoreGetSQL
{
    public class OrdersDbContext : DbContext
    {
        ILoggerProvider _loggerProvider;
        ILoggerFactory _loggerFactory;

        #region Public Properties
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Order> Orders { get; set; }
        #endregion

        #region Constructor
        public OrdersDbContext(ILoggerProvider loggerFactory) 
        {
            _ = Database.EnsureCreated();
            _loggerProvider = loggerFactory;
            _loggerFactory.AddProvider(_loggerProvider);
        }
        #endregion

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
            });

            optionsBuilder.UseLoggerFactory(_loggerFactory);

            var connection = new SqliteConnection("Data Source=Orders.db");
            connection.Open();

            var command = connection.CreateCommand();

            //Create the database if it doesn't already exist
            command.CommandText = "PRAGMA foreign_keys = ON;";
            _ = command.ExecuteNonQuery();
            _ = optionsBuilder.UseSqlite(connection);

            base.OnConfiguring(optionsBuilder);
        }
        #endregion

    }
}
