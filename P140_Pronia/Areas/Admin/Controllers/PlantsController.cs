using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.Helpers;
using P140_Pronia.ViewModels;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlantsController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantsController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Plant> model = _context.Plants.Include(p => p.PlantImages).AsEnumerable();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            PlantCreateVM model = new PlantCreateVM
            {
                Informations = await _context.Informations.ToListAsync(),
                Categories = _context.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlantCreateVM plant)
        {
            PlantCreateVM model = new PlantCreateVM
            {
                Informations = await _context.Informations.ToListAsync(),
                Categories = _context.Categories.ToList()
            };
            //if (!ModelState.IsValid) return View(model);
            Plant newPlant = new Plant();

            bool skuResult = _context.Plants.Any(p => p.SKU.ToLower() == plant.SKU.ToLower());
            if (skuResult)
            {
                ModelState.AddModelError("SKU", "SKU code is duplicated");
                return View(model);
            }
            TempData["test"] = "test";

            #region Main and hover photo
            if (!plant.MainPhoto.IsValidLength(1) || !plant.HoverPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Photo size error");
                return View(model);
            }
            string mainPhotoName = await plant.MainPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
            PlantImage mainImage = new PlantImage
            {
                Path = mainPhotoName,
                IsMain = true
            };
            newPlant.PlantImages.Add(mainImage);

            string hoverPhotoName = await plant.HoverPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
            PlantImage hoverImage = new PlantImage
            {
                Path = hoverPhotoName,
                IsMain = false
            };
            newPlant.PlantImages.Add(hoverImage);

            #endregion

            #region Other photos
            List<string> incorrectFiles = new List<string>();
            foreach (var photo in plant.Photos)
            {
                if (!photo.IsValidLength(1))
                {
                    string fileName = photo.FileName;
                    incorrectFiles.Add(fileName);
                    continue;
                }
                PlantImage other = new PlantImage
                {
                    Path = await photo.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images"),
                    Plant = newPlant
                };
                newPlant.PlantImages.Add(other);
            }
            TempData["IncorrectImages"] = incorrectFiles;
            #endregion

            #region Categories and Informations
            foreach (int id in plant.CategoryIds)
            {
                PlantCategory category = new PlantCategory()
                {
                    CategoryId = id,
                    Plant = newPlant
                };
                newPlant.PlantCategories.Add(category);
            }

            foreach (int id in plant.InformationIds)
            {
                PlantInformation information = new PlantInformation()
                {
                    InformationId = id,
                    Plant = newPlant
                };
                newPlant.PlantInformations.Add(information);
            }
            #endregion

            newPlant.SKU = plant.SKU; 
            newPlant.Name = plant.Name; 
            newPlant.Price = plant.Price; 
            newPlant.Description = plant.Description;

            await _context.Plants.AddAsync(newPlant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
