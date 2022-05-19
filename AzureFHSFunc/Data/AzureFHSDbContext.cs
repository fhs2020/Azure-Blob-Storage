using AzureFHSFunc.Models;

using Microsoft.EntityFrameworkCore;

namespace AzureFHSFunc.Data
{
    public class AzureFHSDbContext : DbContext
    {
        public AzureFHSDbContext(DbContextOptions<AzureFHSDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<SalesRequest> SalesRequests { get; set; }
        public DbSet<GroceryItem> GroceryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
        }
    }
}
