using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Security.Claims;

namespace BookshopWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claims = User.Identity as ClaimsIdentity;
            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                HttpContext.Session.Clear();
                return View(0);
            }

            if (HttpContext.Session.GetInt32(StaticDetails.SESSION_CART) != null)
            {
                return View(HttpContext.Session.GetInt32(StaticDetails.SESSION_CART));
            }
            else
            {
                var count = _unitOfWork.ShoppingCartRepository.GetAll(sc => sc.ApplicationUserId == idClaim.Value).ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.SESSION_CART, count);
                return View(HttpContext.Session.GetInt32(StaticDetails.SESSION_CART));
            }
        }
    }
}
