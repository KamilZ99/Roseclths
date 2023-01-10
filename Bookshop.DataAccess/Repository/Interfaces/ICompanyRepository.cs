using Bookshop.Models;

namespace Bookshop.DataAccess.Repository.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
    }
}
