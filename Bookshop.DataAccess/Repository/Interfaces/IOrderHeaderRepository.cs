using Bookshop.Models;

namespace Bookshop.DataAccess.Repository.Interfaces
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateStripeIds(int id, string sessionId, string paymentIntentId);
    }
}
