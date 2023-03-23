using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Dto.Auth;
using DotnetRpg.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public ActionResult<int> Register([FromBody] UserAccountDto userAccountDto) 
        {
            bool existUser = _authService.ExistUser(userAccountDto.UserName);
            if(!existUser)
            {
                int userId =  _authService.Register(userAccountDto.UserName,userAccountDto.Password);
                return Ok(userId);
            } else {
                return Conflict(new
                {
                    Message = "Invalid username. Please try again."
                });
            }
            
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserAccountDto userAccountDto) 
        {
            string accessToken =_authService.Login(userAccountDto.UserName,userAccountDto.Password);
            return Ok(accessToken);
        }
    }
}