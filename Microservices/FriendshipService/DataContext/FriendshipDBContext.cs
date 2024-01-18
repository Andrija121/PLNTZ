using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FriendshipService.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendshipService.DataContext
{
    public class FriendshipDBContext(DbContextOptions<FriendshipDBContext> options) : DbContext(options)
    {
        public DbSet<Friendship> Friendships {get; set;}

    }
}
