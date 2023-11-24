﻿using Microsoft.AspNetCore.Identity;

namespace UserService.Identity
{
    public class Role:IdentityRole<int>
    {
        public Role() { }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}