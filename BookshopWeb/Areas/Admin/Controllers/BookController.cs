using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookshopWeb.Areas.Admin.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var books = _unitOfWork.BookRepository.GetAll();
            return View(books);
        }

        public IActionResult Upsert(int? id)
        {
            var bookVM = new BookViewModel()
            {
                Book = new Book(),
                
                CategoryList = _unitOfWork.CategoryRepository.GetAll()
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),

                CoverTypeList = _unitOfWork.CoverTypeRepository.GetAll()
                    .Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    })
            };

            if (id is null || id == 0)
            {
                return View(bookVM);
            }
            else
            {

            }

            return View(bookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookViewModel bookVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //_unitOfWork.BookRepository.Update(book);
                _unitOfWork.Save();
                TempData["success"] = "Book updated successfully.";
                return RedirectToAction("Index");
            }
            return View(bookVM);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a book.";
                return RedirectToAction("Index");
            }

            var book = _unitOfWork.BookRepository.GetFirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a book.";
                return RedirectToAction("Index");
            }

            _unitOfWork.BookRepository.Remove(book);
            _unitOfWork.Save();
            TempData["success"] = "Book deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
