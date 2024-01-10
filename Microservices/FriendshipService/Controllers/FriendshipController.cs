﻿using FriendshipService.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetFriendshipsForUser(string userId)
        {
            List<Friendship> friendships = await _friendshipService.GetFriends(userId);
            if(friendships ==null) { return NotFound(); }
            return Ok(friendships) ;
        }
        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendshipRequest([FromBody] FriendshipRequestDto friendshipRequest)
        {
            try
            {
                await _friendshipService.SendFriendshipRequest(friendshipRequest.SenderId, friendshipRequest.RecipientId);
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
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
    }

    public class FriendshipStatusDto
    {
        public bool Accept { get; set; }
    }

}

