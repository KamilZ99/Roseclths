using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookshopWeb.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> productList = _unitOfWork.BookRepository.GetAll(includeProperties: "Category,CoverType");

            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var book = _unitOfWork.BookRepository.GetFirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                TempData["error"] = "An unexpected error occurred while trying to display the book.";
                return RedirectToAction("Index");
            }

            var shoppingCart = new ShoppingCart() { Book = book, Count = 1 };

            return View(shoppingCart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}