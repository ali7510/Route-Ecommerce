using Ecommerce.Domain.IdentityModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<Result<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Error.NotFound("User.NotFound", "User not found");
            }
            return new UserDto(user.Email!, user.DisplayName, await CreateTokenAsync(user)); 
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
            var Token = await CreateTokenAsync(user);
            return new UserDto(user.Email!, user.DisplayName, Token );
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
            var Token = await CreateTokenAsync(user);
            return new UserDto(user.Email!, user.DisplayName, Token); 
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Token => (Issuer, Audience, Claims, Expiration, SigningCredentials)
            // 1- U should get the claims which is the information about each user
            // the claims will be (username, email, role)
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = _configuration["JwtOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],// the issuer is the application that is generating the token (Me)
                audience: _configuration["JwtOptions:Audience"], // the audience is the application that is consuming the token (User)
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
