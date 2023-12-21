using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P140_Pronia.DAL;
using P140_Pronia.Entities;

namespace P140_Pronia.Controllers
{
    public class PlantsController:Controller
    {
        private readonly ProniaDbContext _context;

        public PlantsController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Details(int id)
        {
            if (id == 0) return BadRequest();

            Plant plant = _context.Plants
                                    .Include(p=>p.PlantInformations)
                                    .ThenInclude(p=>p.Information)
                                    .Include(p=>p.PlantCategories)
                                    .ThenInclude(pc=>pc.Category)
                                    .Include(p=>p.PlantImages)
                                    .FirstOrDefault(p=>p.Id == id)!;

            if (plant is null) return NotFound();

            return View(plant);
        }

        public IActionResult GetPlantsPartial(IEnumerable<Plant> Plants)
        {
            return PartialView("_PlantsPv", _context.Plants.Include(p => p.PlantImages).ToList());
        }
    }
}
