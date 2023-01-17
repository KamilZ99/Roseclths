using Bookshop.DataAccess.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Bookshop.Utility;

namespace Bookshop.Models
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
            

            #region Categories

            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category()
                    {
                        Name = "Action"
                    },
                    new Category()
                    {
                        Name = "Fantasy"
                    },
                    new Category()
                    {
                        Name = "Romance"
                    },
                    new Category()
                    {
                        Name = "Drama"
                    },
                    new Category()
                    {
                        Name = "Biography"
                    },
                    new Category()
                    {
                        Name = "Science-Fiction"
                    },
                    new Category()
                    {
                        Name = "Dystopian Novel"
                    }
                };
                
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }


            #endregion

            #region Cover Types

            if (!await context.CoverTypes.AnyAsync())
            {
                var categories = new List<CoverType>
                { 
                    new CoverType()
                    {
                        Name = "Softcover"
                    },
                    new CoverType()
                    {
                        Name = "Hardcover"
                    },
                    new CoverType()
                    {
                        Name = "Hardcover with Dust Jacket"
                    },
                    new CoverType()
                    {
                        Name = "Notebook cover"
                    },
                };

                await context.CoverTypes.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            #endregion

            #region Books

            if (!await context.Books.AnyAsync())
            {
                var books = new List<Book>
                {
                    new Book()
                    {
                        Title = "Harry Potter and the Deathly Hallows",
                        Description = "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J. K. Rowling and the seventh and final novel of the main Harry Potter series. It was released on 21 July 2007 in the United Kingdom by Bloomsbury Publishing, in the United States by Scholastic, and in Canada by Raincoast Books. The novel chronicles the events directly following Harry Potter and the Half-Blood Prince (2005) and the final confrontation between the wizards Harry Potter and Lord Voldemort.",
                        Author = "J. K. Rowling",
                        ISBN = "9780545139700",
                        ImageUrl = "\\img\\books\\35283910-78b9-4712-b5fb-2718bccf7d64.jpg",
                        Price = 39.99,
                        ListPrice = 49.99,
                        CategoryId = context.Categories.First(x => x.Name == "Fantasy").Id,
                        CoverTypeId = context.CoverTypes.First(x => x.Name == "Softcover").Id
                    },
                    new Book()
                    {
                        Title = "The Alchemist",
                        Description = "The Alchemist is a novel by Brazilian author Paulo Coelho which was first published in 1988. Originally written in Portuguese, it became a widely translated international bestseller. An allegorical novel, The Alchemist follows a young Andalusian shepherd in his journey to the pyramids of Egypt, after having a recurring dream of finding a treasure there.",
                        Author = "Paulo Coelho",
                        ISBN = "9788408052944",
                        ImageUrl = "\\img\\books\\b17567fd-9387-41c7-8bd7-ecd51ba8edbf.jpg",
                        Price = 49.99,
                        ListPrice = 39.99,
                        CategoryId = context.Categories.First(x => x.Name == "Drama").Id,
                        CoverTypeId = context.CoverTypes.First(x => x.Name == "Softcover").Id
                    },
                    new Book()
                    {
                        Title = "Gone with the Wind",
                        Description = "Gone with the Wind is a novel by American writer Margaret Mitchell, first published in 1936. The story is set in Clayton County and Atlanta, both in Georgia, during the American Civil War and Reconstruction Era. It depicts the struggles of young Scarlett O'Hara, the spoiled daughter of a well-to-do plantation owner, who must use every means at her disposal to claw her way out of poverty following Sherman's destructive \"March to the Sea\".",
                        Author = "Margaret Mitchell",
                        ISBN = "9780582418059",
                        ImageUrl = "\\img\\books\\e35cc334-d5d7-48e3-819a-bec134cb0e67.jpg",
                        Price = 69.99,
                        ListPrice = 49.99,
                        CategoryId = context.Categories.First(x => x.Name == "Romance").Id,
                        CoverTypeId = context.CoverTypes.First(x => x.Name == "Softcover").Id
                    },
                    new Book()
                    {
                        Title = "1984",
                        Description = "1984 is a dystopian social science fiction novel and cautionary tale by English writer George Orwell. It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime. Thematically, it centres on the consequences of totalitarianism, mass surveillance and repressive regimentation of people and behaviours within society. Orwell, a democratic socialist, modelled the authoritarian state in the novel on Stalinist Russia and Nazi Germany.",
                        Author = "George Orwell",
                        ISBN = "9780582060180",
                        ImageUrl = "\\img\\books\\51f23901-ad07-42ca-a5eb-5ab740823af4.jpg",
                        Price = 19.99,
                        ListPrice = 9.99,
                        CategoryId = context.Categories.First(x => x.Name == "Dystopian Novel").Id,
                        CoverTypeId = context.CoverTypes.First(x => x.Name == "Softcover").Id
                    },
                };
                
                await context.Books.AddRangeAsync(books);
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
