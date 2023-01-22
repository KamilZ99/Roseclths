using Roseclth.Models;

namespace Roseclth.DataAccess.Repository.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        void Update(Material material);
    }
}
