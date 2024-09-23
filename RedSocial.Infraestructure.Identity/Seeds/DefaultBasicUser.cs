using Microsoft.AspNetCore.Identity;
using RedSocial.Core.Application.Enum;
using RedSocial.Infraestructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Infraestructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {

            ApplicationUser defaultuser = new()
            {
                UserName = "Basic",
                Email = "basic@gmail.com",
                FirstName = "Jhon",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultuser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultuser.Email);

                if(user == null)
                {
                   await userManager.CreateAsync(defaultuser, "123Pa$$word!");
                   await userManager.AddToRoleAsync(defaultuser, Roles.BASIC.ToString());
                }
            }
        }
    }
}
