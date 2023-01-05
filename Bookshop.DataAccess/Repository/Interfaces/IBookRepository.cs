using Bookshop.Models;

namespace Bookshop.DataAccess.Repository.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book book);
    }
}
