using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Identity;

namespace UserService.Data
{
    public class UserDBContext(DbContextOptions<UserDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    AuthzId= "6567896db88a4affe7295ec2",
                    Email = "a.hanga@student.fontys.nl",
                    FirstName = "Andrija",
                    LastName = "Hanga",
                    Birthday = new DateOnly(2001, 01, 26),
                    Country="Netherlands",
                    City="Eindhoven",
                    Last_seen = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    AuthzId = "6567896db88a4affe7295ec2123",
                    Email = "a.hanga123@student.fontys.nl",
                    FirstName = "Andrija123",
                    LastName = "Hanga123",
                    Birthday = new DateOnly(2001, 01, 26),
                    Country = "The Netherlands",
                    City = "New York",
                    Last_seen = DateTime.UtcNow,
                    IsActive = true
                }
                );
        }

    }
}