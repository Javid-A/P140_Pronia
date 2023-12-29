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

        [HttpGet]
        public IActionResult Update(int id)
        {
            if(id == 0) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id)!;
            if (category is null) return NotFound(); 
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(int id,Category category)
        {
            if (id == 0) return BadRequest();

            if (!ModelState.IsValid)
            {
                //foreach (var item in ModelState.Values.SelectMany(v => v.Errors))
                //{
                //    ModelState.AddModelError(string.Empty, item.ErrorMessage);
                //}
                return View();
            }

            Category existedCategory = _context.Categories.FirstOrDefault(c=>c.Id == id)!;

            if (existedCategory is null) return NotFound();

            bool existed = _context.Categories.Any(c => c.Name == category.Name);
            if (existed)
            {
                ModelState.AddModelError(string.Empty,"Dont repeat yourself");
                return View();
            }

            _context.Entry(existedCategory).CurrentValues.SetValues(category);

            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return Json(new { status = 405, Message = "Enter valid id" });
            Category category = _context.Categories.FirstOrDefault(c=>c.Id == id)!;
            if (category is null) return Json(new {status =404,Message="Category not found"});

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Json(new { status = 200, Message = "Category was successfully deleted" });
        }
    }
}
