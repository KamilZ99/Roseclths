using Roseclth.Models;

namespace Roseclth.DataAccess.Repository.Interfaces
{
    public interface ITypeRepository : IRepository<Models.Type>
    {
        void Update(Models.Type type);
    }
}
