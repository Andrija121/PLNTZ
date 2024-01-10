using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
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
            var users = await GetAllUsersAsync();

            if (users.Any(u => u.Email == user.Email))
            {
                throw new Exception($"User with EMAIL {user.Email}");
            }
            if (users.Any(u => u.AuthzId == user.AuthzId))
            {
                throw new Exception($"User with EMAIL {user.Email}");
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            //double check _dbContext and dbContext
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.AuthzId == user.AuthzId);

            if (existingUser == null)
            {
                throw new Exception($"User with AuthZId {user.AuthzId} not found");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Birthday = user.Birthday;
            existingUser.Last_seen = user.Last_seen;
            existingUser.Country= user.Country;
            existingUser.City= user.City;
            existingUser.IsActive = user.IsActive;
            existingUser.AuthzId = user.AuthzId;
            existingUser.Email = user.Email;

            await _dbContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> DeleteUserAsyncWithAuthzId(string authzId)
        {
            var user = await _dbContext.Users.FindAsync(authzId) ?? throw new Exception($"User With AuthZID {authzId} not found");
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByAuth0IdAsync(string authzId)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.AuthzId == authzId) ?? throw new Exception($"User With AuthZID {authzId} not found");
            return user
                   ?? throw new Exception($"User with {authzId} was not found");
        }

        public async Task<List<User>> GetAllUsersForCountryAsync(string country)
        {
            List<User> users = await _dbContext.Users.ToListAsync();
            List<User> users_county = new List<User>();
            foreach (User user in users)
            {
                if (user.Country == country)
                {
                    users_county.Add(user);
                }
            }
            return users_county;
        }

        public async Task<List<User>> GetAllUsersForCityAsync(string city)
        {
            List<User> users = await _dbContext.Users.ToListAsync();
            List<User> users_city = new();
            foreach (User user in users)
            {
                if (user.City == city)
                {
                    users_city.Add(user);
                }
            }
            return users_city;
        }

        public Task<User> GetUserByCountry(string country)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByCity(string city)
        {
            throw new NotImplementedException();
        }
    }
}
