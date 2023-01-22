using Roseclth.Models;

namespace Roseclth.DataAccess.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
