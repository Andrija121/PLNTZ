﻿using Microsoft.AspNetCore.Identity;
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
                    Id = 1,
                    AuthzId= "6567896db88a4affe7295ec2",
                    Email = "a.hanga@student.fontys.nl",
                    FirstName = "Andrija",
                    LastName = "Hanga",
                    Birthday = new DateOnly(2001, 01, 26),
                    Last_seen = DateTime.UtcNow,
                    IsActive = true
                }
                );
        }

    }
}