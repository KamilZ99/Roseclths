using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookshopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                await _unitOfWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.Save();
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a category.";
                return RedirectToAction("Index");
            }

            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a category.";
                return RedirectToAction("Index");
            }

            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
