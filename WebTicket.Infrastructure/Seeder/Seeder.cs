using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Enums;
using WebTicket.Domain.Exceptions;
using WebTicket.Domain.Requests;

namespace WebTicket.Infrastructure.Seeder
{
    public static class Seeder
    {
        public static async Task SeedAdminDataAsync(UserManager<User> userManager)
        {
            //nếu đã có tài khoản admin thì không cần seed lại
            var result = await userManager.FindByEmailAsync("caohoangnhat58@gmail.com");
            if (result != null)
            {
                return;
            }
            //tài khoản admin
            var user = User.Create("User0001", string.Empty, "caohoangnhat58@gmail.com", "Tony", "Jugo", "0768608545", "Uni0001");
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "12345678"); 
            await userManager.CreateAsync(user);
            var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);
            //tài khoản moderator
            var user2 = User.Create("User0002", string.Empty, "caohoangnhat59@gmail.com", "Tony", "Jugo", "0768608545", "Uni0001");
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "12345678");
            await userManager.CreateAsync(user2);
            var addRoleResult2 = await userManager.AddToRoleAsync(user, "Moderator");
            await userManager.UpdateAsync(user2);
            //tài khoản organizer
            var user3 = User.Create("User0003", string.Empty, "caohoangnhat60@gmail.com", "Tony", "Jugo", "0768608545", "Uni0001");
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "12345678");
            await userManager.CreateAsync(user3);
            var addRoleResult3 = await userManager.AddToRoleAsync(user, "Moderator");
            await userManager.UpdateAsync(user3);

        }
    }
}
