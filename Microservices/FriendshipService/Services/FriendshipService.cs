using FriendshipService.DataContext;
using FriendshipService.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ;

namespace FriendshipService.Services
{
    public class FriendshipService(FriendshipDBContext friendshipDBContext, RabbitMQConsumer rabbitMQConsumer) : IFriendshipService
    {
        private readonly FriendshipDBContext _dbContext = friendshipDBContext ??throw new ArgumentNullException(nameof(friendshipDBContext));
        private readonly RabbitMQConsumer _rabbitMQConsumer = rabbitMQConsumer ?? throw new ArgumentNullException(nameof(friendshipDBContext));

        public void StartConsuming(string queueName)
        {
            queueName = "plntzq";
            // Subscribe to the specified queue
            _rabbitMQConsumer.Receive(queueName, HandleReceivedMessage);
        }
        private void HandleReceivedMessage(string message)
        {
            // Handle the received message here
            Console.WriteLine($"Received message: {message}");
            // Add your business logic here...
        }
        public async Task<List<Friendship>> GetFriends(string userId)
        {
            var friends = await _dbContext.Friendships.Where(f => (f.User_1_AuthzId == userId || f.User_2_AuthzId == userId)&& f.Status == FriendshipStatus.Accepted).ToListAsync();
            return friends;
        }

        public async Task<List<Friendship>> GetPendingFriends(string userId)
        {
            var friends = await _dbContext.Friendships.Where(f => (f.User_1_AuthzId == userId || f.User_2_AuthzId == userId) && f.Status == FriendshipStatus.Pending).ToListAsync();
            return friends;
        }

        public async Task RespondToFriendshipRequest(int friendShipId,bool accept)
        {
            try
            {
                var friendship = await _dbContext.Friendships.FindAsync(friendShipId) ?? throw new Exception("Friendship not found");
                if (friendship.Status != FriendshipStatus.Pending)
                {
                    // Handle the case where the friendship is not in a pending state
                    throw new Exception("Friendship request is not pending");
                }

                if (accept)
                {
                    friendship.Status = FriendshipStatus.Accepted;
                }
                else if (!accept)
                {
                    friendship.Status = FriendshipStatus.Declined;
                }
                else
                {
                    friendship.Status = FriendshipStatus.Pending;
                }

                // Update the database with the new status
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Handle exceptions or rethrow if necessary
                throw;
            }
        }

        public async Task SendFriendshipRequest(string senderId, string recipientId)
        {
            try
            {
                StartConsuming("plntz");
                var exisitingFriendship = await _dbContext.Friendships.FirstOrDefaultAsync(f=>(f.User_1_AuthzId == senderId && f.User_2_AuthzId==recipientId)||
                (f.User_1_AuthzId==recipientId && f.User_2_AuthzId== senderId));

                if(exisitingFriendship != null)
                {
                    throw new Exception("Friendship request already exists");
                }

                var newFriendship = new Friendship
                {
                    User_1_AuthzId = senderId,
                    User_2_AuthzId = recipientId,
                    Status = FriendshipStatus.Pending,
                };
                _dbContext.Friendships.Add(newFriendship);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
