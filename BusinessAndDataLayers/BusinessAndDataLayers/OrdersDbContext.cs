using DomainLib;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BusinessAndDataLayers
{
    public class OrdersDbContext : DbContext
    {
        public const string ConnectionString = "Data Source=Orders.db";
        #region Public Properties
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderRecord> OrderRecord { get; set; }
        #endregion

        #region Constructor
        public OrdersDbContext() 
        {
            _ = Database.EnsureCreated();
        }
        #endregion

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqliteConnection(ConnectionString);
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
