using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Model;
using WebAPI.Model;
using BLL.DTOModels;
using BLL.ServicesInterfaces;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequestDTO loginDto)
        {
            var success = _userService.Login(loginDto);
            if (!success)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok("Login successful");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return NoContent();
        }
    }
}
