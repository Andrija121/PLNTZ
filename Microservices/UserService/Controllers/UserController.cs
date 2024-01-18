
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

        [HttpGet("{authzId}")]
        public async Task<IActionResult> GetUserByAuthZId(string authzId)
        {
            var user = await _userService.GetUserByAuth0IdAsync(authzId);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpPost("add-friend")]
        public async Task<IActionResult> SendUserIdsForFriendship(string user1Id, string user2Id)
        {
            try
            {
                await _userService.SendUserIdsForFriendship(user1Id, user2Id);
                return Ok("Friendship request sent successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest($"Failed to send friendship request: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetAllUsersForCountry(string country)
        {
            var users = await _userService.GetAllUsersForCountryAsync(country);
            return Ok(users);
        }
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetAllUsersForCity(string city)
        {
            var users = await _userService.GetAllUsersForCityAsync(city);
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
            existingUser.Country = editedUser.Country;
            existingUser.City = editedUser.City;
            existingUser.IsActive = editedUser.IsActive;

            await _userService.UpdateUserAsync(existingUser);

            return NoContent();
        }


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