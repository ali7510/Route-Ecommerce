using Ecommerce.Domain.IdentityModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return Error.InvalidCredintials("User.InvalidCredentials");
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Error.InvalidCredintials("User.InvalidCredentials");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Error.InvalidCredintials("User.InvalidCredentials");
            }
            return new UserDto(user.Email!, user.DisplayName, "token");
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            if(registerDto == null)
            {
                return Error.InvalidCredintials("User.InvalidCredentials");
            }
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return Error.Validation("User.EmailAlreadyExists", "Email already exists");
            }
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var IdentityResult = await _userManager.CreateAsync(user, registerDto.Password);
            if (!IdentityResult.Succeeded)
            {
                return IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            }
            return new UserDto(user.Email!, user.DisplayName, "token"); 
        }
    }
}
