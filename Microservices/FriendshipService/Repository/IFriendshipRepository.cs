using FriendshipService.Models;

namespace FriendshipService.Repository
{
    public interface IFriendshipRepository
    {
        void SendFriendshipRequest(string senderId, string recipientId);
        void RespondToFriendshipRequest(int friendShipId, bool accept);
        IEnumerable<Friendship> GetFriends(string userId);
        IEnumerable<Friendship> GetPendingFriends(string userId);

        void Save();
    }
}
