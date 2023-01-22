using Roseclth.Models;

namespace Roseclth.DataAccess.Repository.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
    }
}
