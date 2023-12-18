
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Identity;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase()
    {
        private readonly IUserService _userService = userService;

        //[HttpGet("{userId}", Name ="Get")]
        //public async Task<IActionResult> GetUserById(int userId)
        //{
        //    var user = await _userService.GetUserByIdAsync(userId);
        //    if (user == null)
        //        return NotFound();

        //    return Ok(user);
        //}
        [HttpGet("{authzId}")]
        public async Task<IActionResult> GetUserByAuthZId(string authzId)
        {
            var user = await _userService.GetUserByAuth0IdAsync(authzId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{authzId}")]
        public async Task<IActionResult> EditUser(string authzId, [FromBody] User editedUser)
        {
            var existingUser = await _userService.GetUserByAuth0IdAsync(authzId);

            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = editedUser.FirstName;
            existingUser.LastName = editedUser.LastName;
            existingUser.Email = editedUser.Email;
            existingUser.Birthday = editedUser.Birthday;
            existingUser.Last_seen = editedUser.Last_seen;
            existingUser.IsActive = editedUser.IsActive;

            await _userService.UpdateUserAsync(existingUser);

            return NoContent();
        }

        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> DeleteUser(int userId)
        //{
        //    var success = await _userService.DeleteUserAsync(userId);

        //    if (success)
        //        return NoContent();

        //    return NotFound();
        //}


        [HttpDelete("{authzId}")]
        public async Task<IActionResult> DeleteUser(string authzId)
        {
            var success = await _userService.DeleteUserAsyncWithAuthzId(authzId);

            if (success)
                return NoContent();

            return NotFound();
        }
    }
}