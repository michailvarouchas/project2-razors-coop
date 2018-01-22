using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models.SeedData
{
    public class SeedUsers
    {
        private const string AdminUsername = "admin@afdemp.gr";
        private const string AdminPassword = "Secret123$";

        private const string MemberUsername1 = "member1@afdemp.gr";
        private const string MemberPassword1 = "Secret123$";

        private const string MemberUsername2 = "member2@afdemp.gr";
        private const string MemberPassword2 = "Secret123$";

        private const string MemberUsername3 = "member3@afdemp.gr";
        private const string MemberPassword3 = "Secret123$";

        private const string UserUsername = "user@afdemp.gr";
        private const string UserPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<ApplicationUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<ApplicationUser>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            ApplicationUser admin = await userManager.FindByNameAsync(AdminUsername);
            ApplicationUser member1 = await userManager.FindByNameAsync(MemberUsername1);
            ApplicationUser member2 = await userManager.FindByNameAsync(MemberUsername2);
            ApplicationUser member3 = await userManager.FindByNameAsync(MemberUsername3);
            ApplicationUser simpleUser = await userManager.FindByNameAsync(UserUsername);

            if (admin == null)
            {
                admin = new ApplicationUser(AdminUsername)
                {
                    Email = AdminUsername
                };
                await userManager.CreateAsync(admin, AdminPassword);
            }

            if (member1 == null)
            {
                member1 = new ApplicationUser(MemberUsername1)
                {
                    Email = MemberUsername1
                };

                await userManager.CreateAsync(member1, MemberPassword1);
            }

            if (member2 == null)
            {
                member2 = new ApplicationUser(MemberUsername2)
                {
                    Email = MemberUsername2
                };

                await userManager.CreateAsync(member2, MemberPassword3);
            }

            if (member3 == null)
            {
                member3 = new ApplicationUser(MemberUsername3)
                {
                    Email = MemberUsername3
                };

                await userManager.CreateAsync(member3, MemberPassword3);
            }

            if (simpleUser == null)
            {
                simpleUser = new ApplicationUser(UserUsername)
                {
                    Email = UserUsername
                };
                await userManager.CreateAsync(simpleUser, UserPassword);
            }

            IdentityRole superAdminRole = await roleManager.FindByNameAsync("SuperAdmin");
            IdentityRole memberRole = await roleManager.FindByNameAsync("Member");
            IdentityRole userRole = await roleManager.FindByNameAsync("User");

            if (superAdminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }

            if (memberRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Member"));

                await userManager.AddToRoleAsync(member1, "Member");
                await userManager.AddToRoleAsync(member2, "Member");
                await userManager.AddToRoleAsync(member3, "Member");
            }

            if (userRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));

                await userManager.AddToRoleAsync(simpleUser, "User");
            }

        }
    }
}
