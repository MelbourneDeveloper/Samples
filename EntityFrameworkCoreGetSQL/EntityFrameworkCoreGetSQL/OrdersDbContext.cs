
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
        public OrdersDbContext(ILoggerProvider loggerProvider) 
        {
            _ = Database.EnsureCreated();
            _loggerProvider = loggerProvider;
            _loggerFactory.AddProvider(_loggerProvider);
        }
        #endregion

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                        //.AddConsole((options) =>
                        //{
                        //    //This displays arguments from the scope
                        //    options.IncludeScopes = true;
                        //})
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
