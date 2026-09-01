using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.IdentityDtos
{
    public record RegisterDto(string DisplayName, [EmailAddress]string Email, string Password, string Username, [Phone]string PhoneNumber);
}
