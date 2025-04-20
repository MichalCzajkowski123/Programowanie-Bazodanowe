using BLL.DTOModels;
using BLL.ServicesInterfaces;
using BLL_MongoDb.MongoModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_MongoDb.MongoServices
{
    public class MongoUserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private static User? _loggedInUser = null;

        public MongoUserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public bool Login(UserLoginRequestDTO loginData)
        {
            if (string.IsNullOrEmpty(loginData.Login) || string.IsNullOrEmpty(loginData.Password))
            {
                throw new ArgumentException("Login and password are required.");
            }

            var filter = Builders<User>.Filter.Eq(u => u.Login, loginData.Login) &
                         Builders<User>.Filter.Eq(u => u.Password, loginData.Password);

            var user = _users.Find(filter).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("User account is inactive.");
            }

            _loggedInUser = user;
            return true;
        }

        public void Logout()
        {
            _loggedInUser = null;
        }

        public static User? GetLoggedInUser()
        {
            return _loggedInUser;
        }
    }
}
