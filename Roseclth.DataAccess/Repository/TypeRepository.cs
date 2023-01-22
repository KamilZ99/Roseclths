using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;

namespace Roseclth.DataAccess.Repository
{
    public class TypeRepository : Repository<Models.Type>, ITypeRepository
    {
        private ApplicationDbContext _context;

        public TypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Models.Type type)
        {
            _context.Types.Update(type);
        }
    }
}
