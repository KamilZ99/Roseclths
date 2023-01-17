using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookshopWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Book> booklist = _unitOfWork.BookRepository.GetAll("Category,CoverType");

            return View(booklist);
        }

        [HttpGet]
        public IActionResult Details(int bookId)
        {
            var book = _unitOfWork.BookRepository.GetFirstOrDefault(b => b.Id == bookId, "CoverType,Category");

            if (book == null)
            {
                TempData["error"] = "Book was not found.";
                return RedirectToAction(nameof(Index));
            }

            var shoppingCart = new ShoppingCart() { BookId = bookId, Book = book, Count = 1 };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            var claims = User.Identity as ClaimsIdentity;
            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            if(idClaim == null) { return Unauthorized(); }
            cart.ApplicationUserId = idClaim.Value;

            var dbCart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault
                (sc => sc.ApplicationUserId == idClaim.Value && sc.BookId == cart.BookId);

            if (dbCart == null)
            {
                _unitOfWork.ShoppingCartRepository.Add(cart);
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value).ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.SESSION_CART, count);
            }

            else
                _unitOfWork.ShoppingCartRepository.IncrementCount(dbCart, cart.Count);
            
            
            TempData["success"] = "The book has been added to the cart";
            
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}