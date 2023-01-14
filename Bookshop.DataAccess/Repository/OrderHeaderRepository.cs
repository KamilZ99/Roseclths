using Bookshop.DataAccess.Data;
using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;

namespace Bookshop.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var dbOrder = _context.OrderHeaders.FirstOrDefault(oh => oh.Id == id);
            
            if (dbOrder == null)
                return;
            
            dbOrder.OrderStatus = orderStatus;
            
            if(paymentStatus != null)
                dbOrder.PaymentStatus = paymentStatus;

            _context.SaveChanges();
            
        }

        public void UpdateStripeIds(int id, string sessionId, string paymentIntentId)
        {
            var dbOrder = _context.OrderHeaders.FirstOrDefault(oh => oh.Id == id);
            
            if (dbOrder == null)
                return;
            
            dbOrder.PaymentDate = DateTime.Now;
            dbOrder.SessionId = sessionId;
            dbOrder.PaymentIntentId = paymentIntentId;
        }
    }
}
