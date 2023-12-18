using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Identity
{
    public class User
    {
        [Key]
        public string AuthzId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public DateOnly Birthday { get; set; }
        public DateTime Last_seen { get; set; }
        public bool IsActive { get; set; } 

        public User()
        {
            IsActive = true;
        }
    }
}