using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Identity;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        //Task<User> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);
        //Task<bool> DeleteUserAsync(int userId);
        Task<bool> DeleteUserAsyncWithAuthzId(string authzId);
        Task<User> GetUserByAuth0IdAsync(string authzId);
    }
}
