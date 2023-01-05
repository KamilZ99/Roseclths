using Bookshop.DataAccess.Data;
using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;

namespace Bookshop.DataAccess.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Book book)
        {
            var bookDB = _context.Books.FirstOrDefault(b => b.Id == book.Id);
            
            if(bookDB == null)
                return;

            bookDB.Title = book.Title;
            bookDB.Description = book.Description;
            bookDB.ISBN = book.ISBN;
            bookDB.Author = book.Author;
            bookDB.ListPrice = book.ListPrice;
            bookDB.Price = book.Price;

            if(book.ImageUrl is not null)
                bookDB.ImageUrl = book.ImageUrl;
        }
    }
}
