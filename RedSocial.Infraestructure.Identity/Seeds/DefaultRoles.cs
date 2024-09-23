using Microsoft.AspNetCore.Identity;
using RedSocial.Core.Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {

            await roleManager.CreateAsync(new IdentityRole(Roles.BASIC.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.SUPERADMIN.ToString()));
        }
    }
}
