using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookshopWeb.Areas.Admin.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var bookViewModel = new BookViewModel()
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
                // create
                return View(bookViewModel);
            }
            else
            {
                // update
            }

            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookViewModel bookViewModel, IFormFile file)
        {
            if(!ModelState.IsValid)
                    return View(bookViewModel);

            var webRootPath = _webHostEnvironment.WebRootPath;

            if (webRootPath == null)
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null.");

            if (file != null)
            {   
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"img\books");
                var extension = Path.GetExtension(file.FileName);

                var fullPath = Path.Combine(uploads, fileName + extension);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                bookViewModel.Book.ImageUrl = @"\images\products\" + fileName + extension;
            }
            
            _unitOfWork.BookRepository.Add(bookViewModel.Book);
            _unitOfWork.Save();
            TempData["success"] = "Book created successfully.";
            
            return RedirectToAction("Index");
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

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _unitOfWork.BookRepository.GetAll();

            return Json(new { data = books });
        }

        #endregion
    }
}
