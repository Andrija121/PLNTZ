using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Identity;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(int userId);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
