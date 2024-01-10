using System.ComponentModel.DataAnnotations;

namespace FriendshipService.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }
        public string? User_1_AuthzId { get; set; }
        public string? User_2_AuthzId { get; set;}
        public FriendshipStatus Status { get; set; }

    }
}
