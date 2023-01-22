using Roseclth.Models;

namespace Roseclth.DataAccess.Repository.Interfaces
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
