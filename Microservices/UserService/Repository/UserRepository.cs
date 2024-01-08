using Microsoft.EntityFrameworkCore;
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
            var users = GetUsers();
            if (users.Any(u => u.Email == user.Email || u.AuthzId == user.AuthzId))
            {
                throw new Exception($"User with EMAIL {user.Email}, or {user.AuthzId} already exists");
            }
            _dbContext.Users.Add(user);
        }

        public void DeleteUserByAuth0Id(string auth0Id)
        {
            var user = _dbContext.Users.Find(auth0Id);
            _dbContext.Users.Remove(user); Save();
        }

        public IEnumerable<User> GetAllUsersForCity(string city)
        {
            List<User> users = _dbContext.Users.ToList().FindAll(u => u.City == city);
            return users;
        }

        public IEnumerable<User> GetAllUsersForCountry(string country)
        {
            List<User> users = _dbContext.Users.ToList().FindAll(u => u.Country == country);
            return users;
        }

        public User GetUserByAuth0Id(string authzId)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.AuthzId == authzId);
            return user;
        }

        public User GetUserByCity(string city)
        {
            User user = _dbContext.Users.FirstOrDefault(u =>u.City == city);
            return user;
        }

        public User GetUserByCountry(string country)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.Country== country);
            return user;
        }

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