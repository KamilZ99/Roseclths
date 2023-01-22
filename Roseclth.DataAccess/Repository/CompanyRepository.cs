using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;

namespace Roseclth.DataAccess.Repository
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
