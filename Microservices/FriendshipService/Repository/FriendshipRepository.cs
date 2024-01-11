using FriendshipService.DataContext;
using FriendshipService.Models;

namespace FriendshipService.Repository
{
    public class FriendshipRepository(FriendshipDBContext friendshipDBContext) : IFriendshipRepository
    {
        private readonly FriendshipDBContext _dbContext = friendshipDBContext ?? throw new ArgumentNullException(nameof(friendshipDBContext));
        public IEnumerable<Friendship> GetFriends(string userId)
        {
            return _dbContext.Friendships.Where(f => (f.User_1_AuthzId == userId || f.User_2_AuthzId==userId) && f.Status == FriendshipStatus.Accepted).ToList();
        }

        public void RespondToFriendshipRequest(int friendshipId, bool accept)
        {
            try
            {
                var friendship = _dbContext.Friendships.Find(friendshipId) ?? throw new InvalidOperationException("Friendship not found");
                if (friendship.Status != FriendshipStatus.Pending)
                {
                    throw new InvalidOperationException("Friendship request is not pending");
                }

                friendship.Status = accept ? FriendshipStatus.Accepted : FriendshipStatus.Declined;

                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SendFriendshipRequest(string senderId, string recipientId)
        {
            try
            {
                var existingFriendship = _dbContext.Friendships
                    .FirstOrDefault(f =>
                        (f.User_1_AuthzId == senderId && f.User_2_AuthzId == recipientId) ||
                        (f.User_1_AuthzId == recipientId && f.User_2_AuthzId == senderId));

                if (existingFriendship != null)
                {
                    // Handle the case where a friendship request already exists
                    throw new InvalidOperationException("Friendship request already exists");
                }

                var newFriendship = new Friendship
                {
                    User_1_AuthzId = senderId,
                    User_2_AuthzId = recipientId,
                    Status = FriendshipStatus.Pending
                };

                _dbContext.Friendships.Add(newFriendship);
                Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Friendship> GetPendingFriends(string userId)
        {
            return _dbContext.Friendships.Where(f => (f.User_1_AuthzId == userId || f.User_2_AuthzId == userId) && f.Status == FriendshipStatus.Pending).ToList();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
