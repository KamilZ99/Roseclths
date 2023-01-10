using Bookshop.DataAccess.Data;
using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;

namespace Bookshop.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }
    }
}
