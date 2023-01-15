using Bookshop.DataAccess.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bookshop.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            
            #region Books

            if (!context.Books.Any())
            {
                new Book()
                {
                    Title = "",
                    Description = "",
                    Author = "",
                    ISBN = "",
                    ImageUrl = "",
                    Price = 0,
                    ListPrice = 0,
                    CategoryId = 0,
                    CoverTypeId = 0
                };
            }

            #endregion

            #region Companies
            
            if (!context.Companies.Any())
            {
                new Company()
                {
                    Name = "",
                    Street = "",
                    City = "",
                    State = "",
                    PostalCode = "",
                    PhoneNumber = ""
                };
            }

            #endregion

            #region MyRegion

            #endregion

        }
    }
    
}
