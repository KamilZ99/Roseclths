using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;

namespace Roseclth.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productDb = _context.Products.FirstOrDefault(b => b.Id == product.Id);
            
            if(productDb == null)
                return;

            productDb.Name = product.Name;
            productDb.Description = product.Description;
            productDb.Brand = product.Brand;
            productDb.ListPrice = product.ListPrice;
            productDb.Price = product.Price;

            if(product.ImageUrl is not null)
                productDb.ImageUrl = product.ImageUrl;

            productDb.TypeId = product.TypeId;
            productDb.MaterialId = product.MaterialId;
        }
    }
}
