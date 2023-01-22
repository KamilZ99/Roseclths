using Roseclth.DataAccess.Repository.Interfaces;
using Roseclth.Models;
using Roseclth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RoseclthWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class TypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var types = _unitOfWork.TypeRepository.GetAll();
            return View(types);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Roseclth.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TypeRepository.Add(type);
                _unitOfWork.Save();
                TempData["success"] = "Type created successfully.";
                return RedirectToAction("Index");
            }
            return View(type);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
                return NotFound();

            var type = _unitOfWork.TypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (type == null)
                return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Roseclth.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.TypeRepository.Update(type);
                _unitOfWork.Save();
                TempData["success"] = "Type updated successfully.";
                return RedirectToAction("Index");
            }
            return View(type);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a type.";
                return RedirectToAction("Index");
            }

            var type = _unitOfWork.TypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (type == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a type.";
                return RedirectToAction("Index");
            }

            _unitOfWork.TypeRepository.Remove(type);
            _unitOfWork.Save();
            TempData["success"] = "Type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
