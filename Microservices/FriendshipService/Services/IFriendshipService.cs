﻿using FriendshipService.Models;
using System.Globalization;

namespace FriendshipService.Services
{
    public interface IFriendshipService
    {
        Task SendFriendshipRequest(string senderId, string recipientId);
        Task RespondToFriendshipRequest(int friendShipId, bool accept);
        Task<List<Friendship>> GetFriends(string userId);
        Task<List<Friendship>>GetPendingFriends(string userId);
    }
}
