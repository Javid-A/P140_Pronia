using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.ViewModels;

namespace P140_Pronia.Controllers
{
    public class PlantsController : Controller
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
                                    .Include(p => p.PlantInformations)
                                    .ThenInclude(p => p.Information)
                                    .Include(p => p.PlantCategories)
                                    .ThenInclude(pc => pc.Category)
                                    .Include(p => p.PlantImages)
                                    .FirstOrDefault(p => p.Id == id)!;

            if (plant is null) return NotFound();

            return View(plant);
        }

        public IActionResult GetPlantsPartial(IEnumerable<Plant> Plants)
        {
            return PartialView("_PlantsPv", _context.Plants.Include(p => p.PlantImages).ToList());
        }


        public IActionResult AddBasket(int id)
        {
            if (id == 0) return NotFound();
            Plant plant = _context.Plants.FirstOrDefault(p => p.Id == id)!;
            if (plant is null) return NotFound();

            //Get cookie
            string basket = HttpContext.Request.Cookies["basket"]!;
            CookieItem cookiePlant = new CookieItem
            {
                Id = plant.Id,
                Name = plant.Name,
                Price = plant.Price,
                Quantity = 1
            };
            BasketItem basketItem = new BasketItem();
            if (basket is null)
            {
                basketItem.CookieItems =  new List<CookieItem>
                {
                    cookiePlant
                };
            }
            else
            {
                basketItem = JsonConvert.DeserializeObject<BasketItem>(basket)!;
                CookieItem existedPlant = basketItem.CookieItems.FirstOrDefault(p => p.Id == id)!;               

                if (existedPlant is null)
                {
                    basketItem.CookieItems.Add(cookiePlant);
                }
                else
                {
                    existedPlant.Quantity++;
                    existedPlant.Price = plant.Price * existedPlant.Quantity;
                }
            }
            basketItem.Count = basketItem.CookieItems.Sum(ci => ci.Quantity);
            basketItem.TotalPrice = basketItem.CookieItems.Sum(ci => ci.Price);

            //Add to cookie
            string plantsStr = JsonConvert.SerializeObject(basketItem);
            HttpContext.Response.Cookies.Append("basket", plantsStr);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowBasket()
        {
            var basket = HttpContext.Request.Cookies["basket"]?? "";
            BasketItem convertedPlant = JsonConvert.DeserializeObject<BasketItem>(basket!)!;
            return Json(convertedPlant);
        }
    }
}
