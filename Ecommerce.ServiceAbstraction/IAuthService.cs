using Ecommerce.Shared.CommonResult;
using Ecommerce.Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IAuthService
    {
        // login (email, password) => token
        // register (email, password, displayName, username) => token

        public Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
        public Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);

        public Task<bool> CheckEmailExistAsync(string email);
        public Task<Result<UserDto>> GetUserByEmail(string email);
    }
}
