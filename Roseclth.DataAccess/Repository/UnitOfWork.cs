using Roseclth.DataAccess.Data;
using Roseclth.DataAccess.Repository.Interfaces;

namespace Roseclth.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            TypeRepository = new TypeRepository(_context);
            MaterialRepository = new MaterialRepository(_context);
            ProductRepository = new ProductRepository(_context);
            CompanyRepository = new CompanyRepository(_context);
            ShoppingCartRepository = new ShoppingCartRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            OrderHeaderRepository = new OrderHeaderRepository(_context);
            OrderDetailsRepository = new OrderDetailsRepository(_context);
        }

        public ITypeRepository TypeRepository { get; }
        public IMaterialRepository MaterialRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public IApplicationUserRepository ApplicationUserRepository { get; set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
