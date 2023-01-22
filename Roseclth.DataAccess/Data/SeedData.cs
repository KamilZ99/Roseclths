using Roseclth.DataAccess.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roseclth.Utility;

namespace Roseclth.Models
{
    public static class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();
            

            #region Types

            if (!await context.Types.AnyAsync())
            {
                var types = new List<Type>
                {
                    new Type()
                    {
                        Name = "Blouse"
                    },
                    new Type()
                    {
                        Name = "Hoodie"
                    },
                    new Type()
                    {
                        Name = "T-Shirt"
                    },
                    new Type()
                    {
                        Name = "Pants"
                    },
                    new Type()
                    {
                        Name = "Socks"
                    },
                    new Type()
                    {
                        Name = "Cap"
                    },
                    new Type()
                    {
                        Name = "Shorts"
                    }
                };
                
                await context.Types.AddRangeAsync(types);
                await context.SaveChangesAsync();
            }


            #endregion

            #region Material

            if (!await context.Materials.AnyAsync())
            {
                var types = new List<Material>
                { 
                    new Material()
                    {
                        Name = "Cotton"
                    },
                    new Material()
                    {
                        Name = "Silk"
                    },
                    new Material()
                    {
                        Name = "Wool"
                    },
                    new Material()
                    {
                        Name = "Linen"
                    },
                };

                await context.Materials.AddRangeAsync(types);
                await context.SaveChangesAsync();
            }

            #endregion

            #region Products

            if (!await context.Products.AnyAsync())
            {
                var products = new List<Product>
                {
                    new Product()
                    {
                        Name = "aBiBas t-shirt",
                        Description = "description max 500 characters",
                        Brand = "Abibas",
                        ImageUrl = "\\img\\products\\abibas.jpg",
                        Price = 39.99,
                        ListPrice = 49.99,
                        TypeId = context.Types.First(x => x.Name == "T-Shirt").Id,
                        MaterialId = context.Materials.First(x => x.Name == "Cotton").Id
                    },
                };
                
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            #endregion

            #region Companies

            if (!await context.Companies.AnyAsync())
            {
                var companies = new List<Company>
                {
                    new Company()
                    {
                        Name = "Shields, Douglas and Ortiz",
                        Street = "9559 Akeem Alley Suite 501",
                        City = "North Daxshire",
                        State = "Nevada",
                        PostalCode = "05705",
                        PhoneNumber = "(593) 583-4968 x014"
                    },
                    new Company()
                    {
                        Name = "Mertz-Altenwerth",
                        Street = "500 Joy Mission Suite 302",
                        City = "New Amiyaton",
                        State = "Wisconsin",
                        PostalCode = "26213-2886",
                        PhoneNumber = "(364) 834-3945"
                    },
                    new Company()
                    {
                        Name = "Gottlieb, Welch and Weimann",
                        Street = "51657 Watsica Mount",
                        City = "Bogisichtown",
                        State = "Kentucky",
                        PostalCode = "74268",
                        PhoneNumber = "887-582-8494 x735"
                    },
                    new Company()
                    {
                        Name = "Fisher Ltd",
                        Street = "701 Abernathy Unions Apt. 127",
                        City = "North Abbie",
                        State = "Georgia",
                        PostalCode = "46520",
                        PhoneNumber = "827.377.8304 x85059"
                    },
                    new Company()
                    {
                        Name = "Parisian LLC",
                        Street = "47317 Elenor Passage Suite 521",
                        City = "New Astridbury",
                        State = "Oregon",
                        PostalCode = "30894-7615",
                        PhoneNumber = "1-389-872-8672 x4331"
                    },
                    new Company()
                    {
                        Name = "Company Name",
                        Street = "Company Street Address",
                        City = "Company City",
                        State = "Company State",
                        PostalCode = "Company Postal",
                        PhoneNumber = "Company phone number"
                    },
                };

                
                await context.Companies.AddRangeAsync(companies);
                await context.SaveChangesAsync();
            }

            #endregion

            #region Roles

            await roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_ADMIN));
            await roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_EMPLOYEE));
            await roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_USER_INDIVIDUAL));
            await roleManager.CreateAsync(new IdentityRole(StaticDetails.ROLE_USER_COMPANY));

            #endregion

            #region Users

            var adminUser = new ApplicationUser()
            {
                Name = "Admin",
                UserName = "admin@wsei.pl",
                NormalizedUserName = "ADMIN@WSEI.PL",
                PhoneNumber = "1234567890",
                Email = "admin@wsei.pl",
                NormalizedEmail = "ADMIN@WSEI.PL",
                EmailConfirmed = true,
                Street = "Admin Street",
                City = "Admin City",
                State = "Admin State",
                PostalCode = "Admin Postal"
            };

            if (!await context.ApplicationUsers.AnyAsync(u => u.Name == adminUser.Name))
            {
                var result = await userManager.CreateAsync(adminUser, "Wsei@1");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, StaticDetails.ROLE_ADMIN);
            }

            var employeeUser = new ApplicationUser()
            {
                Name = "Employee",
                UserName = "employee@wsei.pl",
                NormalizedUserName = "EMPLOYEE@WSEI.PL",
                PhoneNumber = "1234567890",
                Email = "employee@wsei.pl",
                NormalizedEmail = "EMPLOYEE@WSEI.PL",
                EmailConfirmed = true,
                Street = "Employee Street",
                City = "Employee City",
                State = "Employee State",
                PostalCode = "Employee Postal"
            };

            if (!await context.ApplicationUsers.AnyAsync(u => u.Name == employeeUser.Name))
            {
                var result = await userManager.CreateAsync(employeeUser, "Wsei@1");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(employeeUser, StaticDetails.ROLE_EMPLOYEE);
            }

            var companyUser = new ApplicationUser()
            {
                Name = "Company User",
                UserName = "company@wsei.pl",
                NormalizedUserName = "COMPANY@WSEI.PL",
                PhoneNumber = "1234567890",
                Email = "company@wsei.pl",
                NormalizedEmail = "COMPANY@WSEI.PL",
                EmailConfirmed = true,
                Street = "Company Street",
                City = "Company City",
                State = "Company State",
                PostalCode = "Company Postal",
                CompanyId = context.Companies.First(x => x.Name == "Company Name").Id
            };

            if (!await context.ApplicationUsers.AnyAsync(u => u.Name == companyUser.Name))
            {
                var result = await userManager.CreateAsync(companyUser, "Wsei@1");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(companyUser, StaticDetails.ROLE_USER_COMPANY);
            }

            var individualUser = new ApplicationUser()
            {
                Name = "Individual User",
                UserName = "individual@wsei.pl",
                NormalizedUserName = "INDIVIDUAL@WSEI.PL",
                PhoneNumber = "1234567890",
                Email = "individual@wsei.pl",
                NormalizedEmail = "INDIVIDUAL@WSEI.PL",
                EmailConfirmed = true,
                Street = "User Street",
                City = "User City",
                State = "User State",
                PostalCode = "User Postal"
            };

            if (!await context.ApplicationUsers.AnyAsync(u => u.Name == individualUser.Name))
            {
                var result = await userManager.CreateAsync(individualUser, "Wsei@1");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(individualUser, StaticDetails.ROLE_USER_COMPANY);
            }

            await context.SaveChangesAsync();

            #endregion

        }

        public static async Task AssignRole(UserManager<IdentityUser> _userManager, string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return;

            await _userManager.AddToRoleAsync(user, role);
        }

    }

}
