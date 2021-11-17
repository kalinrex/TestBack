using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public  class FirstUser
    {
        public static async Task InsertFirstUser(ActivityDbContext context, UserManager<User> userManager)
        {
            var users = userManager.Users.Count();
            if (users == 0)
            {
                var user = new User { fullName = "Admin", UserName = "Admin", Email = "Admin@gmail.com" };
                await userManager.CreateAsync(user, "Password123$");
            }
        }
    }
}
