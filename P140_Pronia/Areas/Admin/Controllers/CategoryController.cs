using Microsoft.AspNetCore.Mvc;
using P140_Pronia.Areas.Admin.ViewModels;
using P140_Pronia.DAL;
using P140_Pronia.Entities;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ProniaDbContext _context;

        public CategoryController(ProniaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.AsEnumerable();

            return View(categories);
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return BadRequest();

            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();

            return View(category);
        }

        //[HttpGet]
        //[HttpPost]
        //[HttpPut]
        //[HttpPatch]
        //[HttpDelete]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryVM category)
        {
            //var categoryName = Request.Form["category"];

            if (!ModelState.IsValid) return View();

            bool isExisted = _context.Categories.Any(c => c.Name == category.Name);

            if (isExisted) return View();

            Category newCategory = new Category
            {
                Name = category.Name
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
