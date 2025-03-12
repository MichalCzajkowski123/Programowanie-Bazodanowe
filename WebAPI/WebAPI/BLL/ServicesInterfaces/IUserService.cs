using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IUserService
    {
        bool Login(UserLoginRequestDTO loginData);
        void Logout();
    }
}
