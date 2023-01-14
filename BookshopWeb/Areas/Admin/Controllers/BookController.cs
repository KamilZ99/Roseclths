using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookshopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
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
                bookViewModel.Book = _unitOfWork.BookRepository.GetFirstOrDefault(b => b.Id == id);
                return View(bookViewModel);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookViewModel bookViewModel)
        {
            
            if(!ModelState.IsValid)
                return View(bookViewModel);

            var webRootPath = _webHostEnvironment.WebRootPath;

            if (webRootPath == null)
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null.");

            if (bookViewModel.Book.Image != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"img\books");
                var extension = Path.GetExtension(bookViewModel.Book.Image.FileName);

                if (bookViewModel.Book.ImageUrl != null)
                {
                    var path = webRootPath + bookViewModel.Book.ImageUrl;
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                var fullPath = Path.Combine(uploads, fileName + extension);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    bookViewModel.Book.Image.CopyTo(fileStream);
                }

                bookViewModel.Book.ImageUrl = @"\img\books\" + fileName + extension;
            }

            if (bookViewModel.Book.Id == 0)
            {
                _unitOfWork.BookRepository.Add(bookViewModel.Book);
                TempData["success"] = "Book created successfully.";
            }
            else
            {
                _unitOfWork.BookRepository.Update(bookViewModel.Book);
                TempData["success"] = "Book updated successfully.";
            }

            await _unitOfWork.Save();            
            
            return RedirectToAction("Index");
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _unitOfWork.BookRepository.GetAll(includeProperties:"Category,CoverType");

            return Json(new { data = books });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a book.";
                return NotFound();
            }

            var book = _unitOfWork.BookRepository.GetFirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a book.";
                return NotFound();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;
            var path = webRootPath + book.ImageUrl;

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            

            _unitOfWork.BookRepository.Remove(book);
            await _unitOfWork.Save();
            TempData["success"] = "Book deleted successfully.";
            return Ok();
        }

        #endregion
    }
}
