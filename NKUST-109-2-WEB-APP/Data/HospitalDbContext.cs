using NKUST_109_2_WEB_APP.Models;
using Microsoft.EntityFrameworkCore;

namespace NKUST_109_2_WEB_APP.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }
        public DbSet<Hospital> Hospitals { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>().ToTable("Hospital");
        }
    }
}
