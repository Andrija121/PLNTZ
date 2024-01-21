using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Identity;

namespace UserService.Data
{
    public class UserDBContext(DbContextOptions<UserDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Andrija",
                    LastName = "Hanga",
                    AddressId = 1,
                    Birthday = new DateOnly(2001, 01, 26),
                    IsActive = true,
                    Last_seen = DateTime.UtcNow,
                    Password = "andrija123",
                    RoleId = 1,
                }
                );
        }

    }
}