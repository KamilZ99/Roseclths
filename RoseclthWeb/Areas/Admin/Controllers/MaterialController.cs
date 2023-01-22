using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoseclthWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class MaterialController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var materials = _unitOfWork.MaterialRepository.GetAll();
            return View(materials);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.MaterialRepository.Add(material);
                _unitOfWork.Save();
                TempData["success"] = "Material created successfully.";
                return RedirectToAction("Index");
            }
            return View(material);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var material = _unitOfWork.MaterialRepository.GetFirstOrDefault(c => c.Id == id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Material material)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.MaterialRepository.Update(material);
                _unitOfWork.Save();
                TempData["success"] = "Material updated successfully.";
                return RedirectToAction("Index");
            }
            return View(material);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting material.";
                return RedirectToAction("Index");
            }

            var material = _unitOfWork.MaterialRepository.GetFirstOrDefault(c => c.Id == id);

            if (material == null)
            {
                TempData["error"] = "An unexpected error occured while deleting material.";
                return RedirectToAction("Index");
            }

            _unitOfWork.MaterialRepository.Remove(material);
            _unitOfWork.Save();
            TempData["success"] = "Material deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
