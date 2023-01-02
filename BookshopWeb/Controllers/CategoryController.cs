using BookshopWeb.Data;
using BookshopWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookshopWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
