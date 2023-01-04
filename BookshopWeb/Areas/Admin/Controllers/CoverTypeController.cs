using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookshopWeb.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var coverTypes = _unitOfWork.CoverTypeRepository.GetAll();
            return View(coverTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type created successfully.";
                return RedirectToAction("Index");
            }
            return View(coverType);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null)
                return NotFound();

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type updated successfully.";
                return RedirectToAction("Index");
            }
            return View(coverType);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a cover type.";
                return RedirectToAction("Index");
            }

            var coverType = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a cover type.";
                return RedirectToAction("Index");
            }

            _unitOfWork.CoverTypeRepository.Remove(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
