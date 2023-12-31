﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Identity
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Password { get; set; } 
        public DateOnly Birthday { get; set; }
        public DateTime Last_seen { get; set; }
        public bool IsActive { get; set; } 

        public int RoleId { get; set; } 
        public int AddressId { get; set; } 
        public User()
        {
            IsActive = true;
        }
    }
}