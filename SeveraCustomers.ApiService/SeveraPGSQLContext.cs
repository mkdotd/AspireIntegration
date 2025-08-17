using Microsoft.EntityFrameworkCore;
namespace SeveraCustomers.ApiService
{
    public class SeveraPGSQLContext : DbContext
    {
        public SeveraPGSQLContext(DbContextOptions<SeveraPGSQLContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Customers> Customers { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Customers>().ToTable("Customers");
            modelBuilder.Entity<Models.Customers>().HasKey(c => c.ID);
            modelBuilder.Entity<Models.Customers>().HasData(
                new Models.Customers
                {
                    ID = 1,
                    Name = "John Doe",
                    Website = "test.com",
                    IndustryName = "Technology",
                    Number = 1234,
                    OwnerName = "John Smith"
                });
        }
    }
}
