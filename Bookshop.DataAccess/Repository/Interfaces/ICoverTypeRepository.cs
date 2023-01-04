using Bookshop.Models;

namespace Bookshop.DataAccess.Repository.Interfaces
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
