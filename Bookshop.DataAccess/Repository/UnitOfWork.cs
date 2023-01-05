using Bookshop.DataAccess.Data;
using Bookshop.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public ICategoryRepository CategoryRepository { get; }
        public ICoverTypeRepository CoverTypeRepository { get; }
        public IBookRepository BookRepository { get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
