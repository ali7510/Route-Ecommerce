using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.IdentityModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task initializeDataAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        DisplayName = "Ali",
                        UserName = "ali",
                        Email = "ali@gmail.com",
                        PhoneNumber = "01212649609",
                    };
                    var user02 = new ApplicationUser()
                    {
                        DisplayName = "Ahmed",
                        UserName = "ahmed",
                        Email = "ahmed@gmail.com",
                        PhoneNumber = "01212644609",
                    };
                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "SuperAdmin");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while seeding identity data.", ex);
            }
        }
    }
}
