using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Identity;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsyncWithAuthzId(string authzId);
        Task<User> GetUserByAuth0IdAsync(string authzId);
        Task<List<User>> GetAllUsersForCountryAsync(string country);
        Task<List<User>> GetAllUsersForCityAsync(string city);
        Task<User> GetUserByCountry(string country);
        Task<User> GetUserByCity(string city);
        Task SendUserIdsForFriendship(string user1Id, string user2Id);
    }
}
