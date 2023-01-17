using Bookshop.DataAccess.Repository.Interfaces;
using Bookshop.Models;
using Bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookshopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.ROLE_ADMIN)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var company = new Company();

            if (id is null || id == 0)
            {
                // create
                return View(company);
            }
            else
            {
                // update
                company = _unitOfWork.CompanyRepository.GetFirstOrDefault(b => b.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            
            if(!ModelState.IsValid)
                return View(company);

            if (company.Id == 0)
            {
                _unitOfWork.CompanyRepository.Add(company);
                TempData["success"] = "Company created successfully.";
            }
            else
            {
                _unitOfWork.CompanyRepository.Update(company);
                TempData["success"] = "Company updated successfully.";
            }

            _unitOfWork.Save();            
            
            return RedirectToAction("Index");
        }

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.GetAll();

            return Json(new { data = companies });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                TempData["error"] = "An unexpected error occured while deleting a company.";
                return NotFound();
            }

            var company = _unitOfWork.CompanyRepository.GetFirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                TempData["error"] = "An unexpected error occured while deleting a company.";
                return NotFound();
            }

            _unitOfWork.CompanyRepository.Remove(company);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully.";
            return Ok();
        }

        #endregion
    }
}
