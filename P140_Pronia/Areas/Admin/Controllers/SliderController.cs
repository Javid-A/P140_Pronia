using Microsoft.AspNetCore.Mvc;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.Helpers;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(ProniaDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> model = _context.Sliders.AsEnumerable();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if ((double)slider.Photo.Length / 1024 / 1024 > 1 || (double)slider.BgPhoto.Length / 1024 / 1024 > 1)
            {
                ModelState.AddModelError(string.Empty, "Please choose image file which size less than 1MB");
                return View();
            }
           
            slider.Image = await slider.Photo.GeneratePhoto(_env.WebRootPath,"assets","images","website-images");
            slider.BgImage = await slider.BgPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images"); ;

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
