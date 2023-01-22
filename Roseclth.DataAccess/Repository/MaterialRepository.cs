using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;

namespace Roseclth.DataAccess.Repository
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        private ApplicationDbContext _context;

        public MaterialRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Material material)
        {
            _context.Materials.Update(material);
        }
    }
}
