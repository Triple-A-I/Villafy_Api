using Microsoft.EntityFrameworkCore;
using Villafy_Api.Models;

namespace Villafy_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<VillaNumber>().HasData(
            //    new List<VillaNumber>() {
            //   new  VillaNumber{ VillaNo = 101, SpecialDetails = "Details 1", CreatedDate = DateTime.Now },
            //   new  VillaNumber{ VillaNo = 102, SpecialDetails = "Details 2", CreatedDate = DateTime.Now },
            //    }
            //    );
            //modelBuilder.Entity<Blogg>()
            //    .HasOne(b => b.BlogImage)
            //    .WithOne(i => i.Blogg)
            //    .HasForeignKey<BlogImage>(b => b.BloggFK);
        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
    }
}
