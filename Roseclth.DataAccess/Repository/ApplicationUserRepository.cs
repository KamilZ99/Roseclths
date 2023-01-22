using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;

namespace Roseclth.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
