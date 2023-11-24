
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Identity;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService) : ControllerBase()
    {
        private readonly IUserService _userService = userService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> EditUser(int userId, [FromBody] ApplicationUser editedUser)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);

            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = editedUser.FirstName;
            existingUser.LastName = editedUser.LastName;
            existingUser.Birthday = editedUser.Birthday;
            existingUser.Last_seen = editedUser.Last_seen;
            existingUser.IsActive = editedUser.IsActive;
            existingUser.RoleId = editedUser.RoleId;
            existingUser.AddressId = editedUser.AddressId;

            await _userService.UpdateUserAsync(existingUser);

            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromQuery]int userId)
        {
            var success = await _userService.DeleteUserAsync(userId);

            if (success)
                return NoContent();

            return NotFound();
        }
    }
}