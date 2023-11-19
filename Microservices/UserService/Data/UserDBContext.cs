using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Identity;

namespace UserService.Data
{
    public class UserDBContext(DbContextOptions<UserDBContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityRole<int>> IdentityRoles { get; set; }
        public DbSet<IdentityUserRole<int>> IdentityUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure IdentityUserRole<int> as keyless
            modelBuilder.Entity<IdentityUserRole<int>>().HasNoKey();
        }

    }
}