using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Models.ViewModels;
using Bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

#nullable disable

namespace BookshopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderViewModel = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == orderId, "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(od => od.OrderId == orderId, "Book")
            };

            return View(OrderViewModel);
        }

        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details_Customer()
        {
            OrderViewModel.OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == OrderViewModel.OrderHeader.Id, "ApplicationUser");
            OrderViewModel.OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(od => od.OrderId == OrderViewModel.OrderHeader.Id, "Book");

            var domain = "https://localhost:7009/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>()
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderViewModel.OrderHeader.Id}",
            };

            var details = OrderViewModel.OrderDetails.ToList();

            foreach (var detail in OrderViewModel.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(detail.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = detail.Book.Title
                        },
                    },
                    Quantity = detail.Count,
                };

                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeaderRepository.UpdateStripeIds(OrderViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
            await _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        {
            var orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == orderHeaderId);

            if (orderHeader == null) throw new ArgumentNullException(nameof(orderHeader));

            if (orderHeader.PaymentStatus == StaticDetails.PAYMENT_STATUS_DELAYED_PAYMENT)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRepository.UpdateStripeIds(orderHeaderId, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, StaticDetails.PAYMENT_STATUS_APPROVED);
                    await _unitOfWork.Save();
                }
            }

            return View(orderHeaderId);
        }

        [HttpPost]
        [Authorize(Roles = $"{StaticDetails.ROLE_ADMIN},{StaticDetails.ROLE_EMPLOYEE}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var dbOrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == OrderViewModel.OrderHeader.Id);

            if(dbOrderHeader == null) return NotFound("Cannot find this order in database.");

            dbOrderHeader.Name = OrderViewModel.OrderHeader.Name;
            dbOrderHeader.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
            dbOrderHeader.Street = OrderViewModel.OrderHeader.Street;
            dbOrderHeader.City = OrderViewModel.OrderHeader.City;
            dbOrderHeader.State = OrderViewModel.OrderHeader.State;
            dbOrderHeader.PostalCode = OrderViewModel.OrderHeader.PostalCode;

            if (OrderViewModel.OrderHeader.Carrier != null)
            {
                dbOrderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
            }
            
            if (OrderViewModel.OrderHeader.TrackingNumber != null)
            {
                dbOrderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeaderRepository.Update(dbOrderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Details updated succesfully.";
            return RedirectToAction("Details", "Order", new { orderId = dbOrderHeader.Id });

        }

        [HttpPost]
        [Authorize(Roles = $"{StaticDetails.ROLE_ADMIN},{StaticDetails.ROLE_EMPLOYEE}")]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderViewModel.OrderHeader.Id, StaticDetails.STATUS_IN_PROCESS);

            _unitOfWork.Save();
            TempData["success"] = "Order Status updated succesfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{StaticDetails.ROLE_ADMIN},{StaticDetails.ROLE_EMPLOYEE}")]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == OrderViewModel.OrderHeader.Id);

            if(orderHeader == null) return NotFound("Cannot find this order in database.");

            orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
            orderHeader.OrderStatus = StaticDetails.STATUS_SHIPPED;
            orderHeader.ShippingDate = DateTime.Now;

            if (orderHeader.PaymentStatus == StaticDetails.PAYMENT_STATUS_DELAYED_PAYMENT)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }

            _unitOfWork.OrderHeaderRepository.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order shipped succesfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = $"{StaticDetails.ROLE_ADMIN},{StaticDetails.ROLE_EMPLOYEE}")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var dbOrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(oh => oh.Id == OrderViewModel.OrderHeader.Id);

            if(dbOrderHeader == null) return NotFound("Cannot find this order in database.");

            if (dbOrderHeader.PaymentStatus == StaticDetails.PAYMENT_STATUS_APPROVED)
            {
                var options = new RefundCreateOptions()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = dbOrderHeader.PaymentIntentId
                };

                var service = new RefundService();
                var refund = service.Create(options);

                _unitOfWork.OrderHeaderRepository.UpdateStatus(dbOrderHeader.Id, StaticDetails.STATUS_CANCELLED, StaticDetails.STATUS_REFUNDED);
            }
            else
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(dbOrderHeader.Id, StaticDetails.STATUS_CANCELLED, StaticDetails.STATUS_CANCELLED);
            }

            _unitOfWork.Save();
            TempData["success"] = "Order cancelled succesfully.";
            return RedirectToAction("Details", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }

        #region API

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(StaticDetails.ROLE_ADMIN) || User.IsInRole(StaticDetails.ROLE_EMPLOYEE))
            {
                orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll("ApplicationUser");
            }
            else
            {
                var claims = User.Identity as ClaimsIdentity;
                var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim == null) { return Unauthorized(); }

                orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(oh => oh.ApplicationUserId == idClaim.Value, "ApplicationUser");
            }

            switch (status)
            {
                case "inprocess":
                    orderHeaders = orderHeaders.Where(oh => oh.OrderStatus == StaticDetails.STATUS_IN_PROCESS);
                    break;

                case "pending":
                    orderHeaders = orderHeaders.Where(oh => oh.PaymentStatus == StaticDetails.PAYMENT_STATUS_PENDING);
                    break;

                case "completed":
                    orderHeaders = orderHeaders.Where(oh => oh.OrderStatus == StaticDetails.STATUS_SHIPPED );
                    break;

                case "approved":
                    orderHeaders = orderHeaders.Where(oh => oh.OrderStatus == StaticDetails.STATUS_APPROVED );
                    break;

                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}
