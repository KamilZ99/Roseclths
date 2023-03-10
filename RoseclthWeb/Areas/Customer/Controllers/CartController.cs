using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Models.ViewModels;
using Roseclth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace RoseclthWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        public int TotalPrice { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claims = User.Identity as ClaimsIdentity;
            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) { return Unauthorized(); }

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value, "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                ShoppingCartViewModel.OrderHeader.TotalPrice += cart.Price * cart.Count;
            }

            return View(ShoppingCartViewModel);
        }

        [HttpGet]
        [ActionName("Summary")]
        public IActionResult ShowSummary()
        {
            var claims = User.Identity as ClaimsIdentity;
            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) { return Unauthorized(); }

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value, "Product"),
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser =
                _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == idClaim.Value);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.Street = ShoppingCartViewModel.OrderHeader.ApplicationUser.Street;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                ShoppingCartViewModel.OrderHeader.TotalPrice += cart.Price * cart.Count;
            }

            return View(ShoppingCartViewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary()
        {
            var claims = User.Identity as ClaimsIdentity;
            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null) { return Unauthorized(); }

            ShoppingCartViewModel.ListCart = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value, "Product");

            ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartViewModel.OrderHeader.ApplicationUserId = idClaim.Value;

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price);

                ShoppingCartViewModel.OrderHeader.TotalPrice += cart.Price * cart.Count;
            }

            var user = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == idClaim.Value);

            if (user == null) { return Unauthorized(); }

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PAYMENT_STATUS_PENDING;
                ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.STATUS_PENDING;
            }
            else
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PAYMENT_STATUS_DELAYED_PAYMENT;
                ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.STATUS_APPROVED;
            }

            _unitOfWork.OrderHeaderRepository.Add(ShoppingCartViewModel.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartViewModel.ListCart)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };

                _unitOfWork.OrderDetailsRepository.Add(orderDetails);
                _unitOfWork.Save();
            }

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7009/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>()
                    {
                        "card"
                    },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                };

                foreach (var cart in ShoppingCartViewModel.ListCart)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cart.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cart.Product.Name
                            },
                        },
                        Quantity = cart.Count,
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                _unitOfWork.OrderHeaderRepository.UpdateStripeIds(ShoppingCartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartViewModel.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == id);

            if (orderHeader == null) throw new ArgumentNullException(nameof(orderHeader));

            if (orderHeader.PaymentStatus != StaticDetails.PAYMENT_STATUS_DELAYED_PAYMENT)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRepository.UpdateStripeIds(id, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeaderRepository.UpdateStatus(id, StaticDetails.STATUS_APPROVED, StaticDetails.PAYMENT_STATUS_APPROVED);
                    _unitOfWork.Save();
                }
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            HttpContext.Session.Clear();

            _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(sc => sc.Id == cartId);
            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(sc => sc.Id == cartId);

            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(sc => sc.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();

            var count = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(StaticDetails.SESSION_CART, count);

            return RedirectToAction(nameof(Index));
        }

        public static double GetPriceBasedOnQuantity(double quantity, double price)
        {
            if(quantity <= 0 || price <= 0)
                throw new ArgumentException();

            if (quantity <= 50)
                return price;

            else if (quantity <= 100)
                return price * 0.85;

            else
                return price * 0.75;
        }
    }
}
