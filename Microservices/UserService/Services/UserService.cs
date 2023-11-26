using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Identity;

namespace UserService.Services
{
    public class UserService(UserDBContext dbContext) : IUserService
    {
        private readonly UserDBContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<User> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            return user ?? throw new Exception($"User with {userId} was not found");
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _dbContext.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                throw new Exception($"User with ID {user.Id} not found");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Birthday = user.Birthday;
            existingUser.Last_seen = user.Last_seen;
            existingUser.IsActive = user.IsActive;
            existingUser.RoleId = user.RoleId;
            existingUser.AddressId = user.AddressId;

            await _dbContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId) ?? throw new Exception($"User with ID {userId} not found");
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
