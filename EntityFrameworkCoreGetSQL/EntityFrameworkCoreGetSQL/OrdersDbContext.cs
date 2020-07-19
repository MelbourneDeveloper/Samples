
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCoreGetSQL
{
    public class OrdersDbContext : DbContext
    {
        #region Fields
        ILoggerProvider _loggerProvider;
        ILoggerFactory _loggerFactory;
        #endregion

        #region Public Properties
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Order> Orders { get; set; }
        #endregion

        #region Constructor
        public OrdersDbContext(ILoggerFactory loggerFactory) 
        {
            _loggerFactory = loggerFactory;
            _ = Database.EnsureCreated();
        }
        #endregion

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
