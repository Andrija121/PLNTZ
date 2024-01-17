using FriendshipService.Models;
using System.Globalization;

namespace FriendshipService.Services
{
    public interface IFriendshipService
    {
        Task SendFriendshipRequest();
        Task RespondToFriendshipRequest(int friendShipId, bool accept);
        Task<List<Friendship>> GetFriends(string userId);
        Task<List<Friendship>>GetPendingFriends(string userId);
    }
}
