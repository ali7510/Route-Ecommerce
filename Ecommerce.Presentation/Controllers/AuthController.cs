using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.IdentityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return HandleResult(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return HandleResult(result);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _authService.CheckEmailExistAsync(email);
            return Ok(result);
        }

        [HttpGet("currentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authService.GetUserByEmail(email!);
            return HandleResult(result);
        }
    }
}
