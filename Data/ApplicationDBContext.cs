using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaim.Models; // Required for Claim

namespace ContractMonthlyClaim.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Claim> Claims { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
