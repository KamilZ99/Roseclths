using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Models.ViewModels;
using Roseclth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RoseclthWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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
            var productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                
                TypeList = _unitOfWork.TypeRepository.GetAll()
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),

                MaterialList = _unitOfWork.MaterialRepository.GetAll()
                    .Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    })
            };

            if (id is null || id == 0)
            {
                // create
                return View(productViewModel);
            }
            else
            {
                // update
                productViewModel.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(b => b.Id == id);
                return View(productViewModel);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            
            if(!ModelState.IsValid)
                return View(productViewModel);

            var webRootPath = _webHostEnvironment.WebRootPath;

            if (webRootPath == null)
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null.");

            if (productViewModel.Product.Image != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"img\products");
                var extension = Path.GetExtension(productViewModel.Product.Image.FileName);

                if (productViewModel.Product.ImageUrl != null)
                {
                    var path = webRootPath + productViewModel.Product.ImageUrl;
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                var fullPath = Path.Combine(uploads, fileName + extension);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    productViewModel.Product.Image.CopyTo(fileStream);
                }

                productViewModel.Product.ImageUrl = @"\img\products\" + fileName + extension;
            }

            if (productViewModel.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productViewModel.Product);
                TempData["success"] = "Product created successfully.";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(productViewModel.Product);
                TempData["success"] = "Product updated successfully.";
            }

            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties:"Type,Material");

            return Json(new { data = products });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a product.";
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.GetFirstOrDefault(b => b.Id == id);

            if (product == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a product.";
                return NotFound();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;
            var path = webRootPath + product.ImageUrl;

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully.";
            return Ok();
        }

        #endregion
    }
}
