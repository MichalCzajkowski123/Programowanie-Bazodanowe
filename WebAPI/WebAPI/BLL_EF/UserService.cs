using BLL.DTOModels;
using BLL.ServicesInterfaces;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_EF
{
    public class UserService : IUserService
    {
        private readonly WebStoreContext _context;

        private static User _loggedInUser = null;

        public UserService(WebStoreContext context)
        {
            _context = context;
        }

        public bool Login(UserLoginRequestDTO loginData)
        {
            if (string.IsNullOrEmpty(loginData.Login) || string.IsNullOrEmpty(loginData.Password))
            {
                throw new ArgumentException("Login and password are required.");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Login == loginData.Login && u.Password == loginData.Password);

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

        public static User GetLoggedInUser()
        {
            return _loggedInUser;
        }
    }
}
