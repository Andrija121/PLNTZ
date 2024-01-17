using FriendshipService.Models;
using FriendshipService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FriendshipService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipController(IFriendshipService friendshipService) : ControllerBase
    {
        private readonly IFriendshipService _friendshipService = friendshipService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFriendshipsForUser(string userId)
        {
            List<Friendship> friendships = await _friendshipService.GetFriends(userId);
            if(friendships ==null) { return NotFound(); }
            return Ok(friendships) ;
        }
        [HttpGet("pending/{userId}")]
        public async Task<IActionResult> GetPendingFriendshipsForUser(string userId)
        {
            List<Friendship> friendships = await _friendshipService.GetPendingFriends(userId);
            if (friendships == null) { return NotFound(); }
            return Ok(friendships);
        }
        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendshipRequest()
        {
            try
            {
                await _friendshipService.SendFriendshipRequest();
                return Ok("Friendship request sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpPost("respond-request/{friendshipId}")]
        public async Task<IActionResult> RespondToFriendshipRequest(int friendshipId, [FromBody] FriendshipStatusDto response)
        {
            try
            {
                await _friendshipService.RespondToFriendshipRequest(friendshipId, response.Accept);
                return Ok("Friendship request responded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }

    public class FriendshipRequestDto
    {
    }

    public class FriendshipStatusDto
    {
        public bool Accept { get; set; }
    }

}

