using Microsoft.EntityFrameworkCore;
using Villafy_Api.Models;

namespace Villafy_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Amenity = "",
                    Details = "",
                    ImageUrl = "",
                    Name = "Royal View",
                    Occupancy = 4,
                    Rate = 7,
                    Sqft = 350
                }, new Villa
                {
                    Id = 2,
                    Amenity = "",
                    Details = "",
                    ImageUrl = "",
                    Name = "Beach View",
                    Occupancy = 6,
                    Rate = 9,
                    Sqft = 650
                }

                );
        }
    }
}
