using Bookshop.DataAccess.Data;
using Bookshop.DataAccess.Repository.Interfaces;

namespace Bookshop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            CoverTypeRepository = new CoverTypeRepository(_context);
            BookRepository = new BookRepository(_context);
            CompanyRepository = new CompanyRepository(_context);
            ShoppingCartRepository = new ShoppingCartRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            OrderHeaderRepository = new OrderHeaderRepository(_context);
            OrderDetailsRepository = new OrderDetailsRepository(_context);
        }

        public ICategoryRepository CategoryRepository { get; }
        public ICoverTypeRepository CoverTypeRepository { get; }
        public IBookRepository BookRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public IApplicationUserRepository ApplicationUserRepository { get; set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; set; }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
