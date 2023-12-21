using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.ViewModels;

namespace P140_Pronia.Controllers
{
    public class HomeController:Controller
    {
        private readonly ProniaDbContext _context;

        public HomeController(ProniaDbContext context)
        {
            _context = context;
        }

        //ContenResult,JsonResult,ViewResult,ActionResult
        public IActionResult Index()
        {
            HomeVM model = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                Plants = _context.Plants.Include(p=>p.PlantImages).ToList()
            };

            if (model.Sliders is null || model.Plants is null) return NotFound();
            
            return View(model);
        }
    }
}
