﻿using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserService.Repository
{
    public class UserRepository(UserDBContext dbContext) : IUserRepository
    {
        private readonly UserDBContext _dbContext = dbContext;

        public void CreateUser(User user)
        {
            _dbContext.Users.Add(user);
        }

        public void DeleteUser(int user_id)
        {
            var user = _dbContext.Users.Find(user_id);
            _dbContext.Users.Remove(user);
            Save();
        }


        public User GetUserById(int user_id) => _dbContext.Users.Find(user_id);

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            Save();
        }
    }
}