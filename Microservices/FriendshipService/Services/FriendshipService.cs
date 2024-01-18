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

        private string _currentSenderId;
        private string _currentRecipientId;

        public void StartConsuming(string queueName)
        {
            queueName = "plntzq";
            // Subscribe to the specified queue
            _rabbitMQConsumer.Receive(queueName, HandleReceivedMessage);
        }
        private void HandleReceivedMessage(string message)
        {
            Console.WriteLine($"Received message: {message}");

            SetUserIdsFromMessage(message);
        }

        private void SetUserIdsFromMessage(string message)
        {
            //double check later on
            string[] userIds = message.Split(new[] {"," }, StringSplitOptions.RemoveEmptyEntries);

            if (userIds.Length == 2)
            {
                _currentSenderId = userIds[0].Trim();
                _currentRecipientId = userIds[1].Trim();
            }
            else
            {
                Console.WriteLine("Invalid message format. Expected format: 'user1id, user2id'");
            }
        }
        public async Task SendFriendshipRequest()
        {
            try
            {
                StartConsuming("plntz");
                await WaitForUserIdsFromMessage();
                string senderId = _currentSenderId;
                string recipientId = _currentRecipientId;
                await Console.Out.WriteLineAsync("SID" + senderId);
                await Console.Out.WriteLineAsync("RID" + recipientId);
                var exisitingFriendship = await _dbContext.Friendships.FirstOrDefaultAsync(f => (f.User_1_AuthzId == senderId && f.User_2_AuthzId == recipientId) ||
                (f.User_1_AuthzId == recipientId && f.User_2_AuthzId == senderId));

                if (exisitingFriendship != null)
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
        private async Task WaitForUserIdsFromMessage()
        {
            // Use a loop to wait until the user IDs are set
            while (_currentSenderId == null || _currentRecipientId == null)
            {
                // You can add a small delay to avoid high CPU usage
                await Task.Delay(100);
            }
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
        public async Task<bool> DeleteFriendshipsForUser(string authzId)
        {
            var friendshipsForUser = await _dbContext.Friendships.Where(f => f.User_1_AuthzId == authzId || f.User_2_AuthzId == authzId).FirstOrDefaultAsync();
            if (friendshipsForUser != null)
            {
                _dbContext.Friendships.Remove(friendshipsForUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

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

                friendship.Status = accept ? FriendshipStatus.Accepted : FriendshipStatus.Declined;

                // Update the database with the new status
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Handle exceptions or rethrow if necessary
                throw;
            }
        }


    }
}
