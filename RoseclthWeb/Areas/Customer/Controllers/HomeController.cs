using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Models.ViewModels;
using Roseclth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace RoseclthWeb.Areas.Customer.Controllers
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
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll("Type,Material");

            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int productId)
        {
            var product = _unitOfWork.ProductRepository.GetFirstOrDefault(b => b.Id == productId, "Material,Type");

            if (product == null)
            {
                TempData["error"] = "Product was not found.";
                return RedirectToAction(nameof(Index));
            }

            var shoppingCart = new ShoppingCart() { ProductId = productId, Product = product, Count = 1 };

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
                (sc => sc.ApplicationUserId == idClaim.Value && sc.ProductId == cart.ProductId);

            if (dbCart == null)
            {
                _unitOfWork.ShoppingCartRepository.Add(cart);
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value).ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.SESSION_CART, count);
            }

            else
                _unitOfWork.ShoppingCartRepository.IncrementCount(dbCart, cart.Count);
            
            
            TempData["success"] = "The product has been added to the cart";
            
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}