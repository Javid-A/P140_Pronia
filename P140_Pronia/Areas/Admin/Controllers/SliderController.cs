using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P140_Pronia.DAL;
using P140_Pronia.Entities;
using P140_Pronia.Helpers;
using P140_Pronia.ViewModels;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator")]
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
            if (!slider.Photo.IsValidLength(1) || !slider.BgPhoto.IsValidLength(1))
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

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == 0) return BadRequest();

            SliderUpdateVM slider = _context.Sliders.Select(s=>new SliderUpdateVM()
            {
                Id = s.Id,
                Image = s.Image,
                BgImage = s.BgImage,
                Title = s.Title,
                Offer = s.Offer,
                ShortDesc = s.ShortDesc,
                URL = s.URL

            }).FirstOrDefault(s=>s.Id == id)!;
            if (slider is null) return NotFound();

            return View(slider);
        }

        

        [HttpPost]
        public async Task<IActionResult> Update(int id,SliderUpdateVM updatedSlider)
        {
            //bool Test(Slider slider)
            //{
            //    return slider.Id==id;
            //}
            if (id == 0) return BadRequest();
            //Slider slider = _context.Sliders.FirstOrDefault(Test)!;
            Slider slider = _context.Sliders.FirstOrDefault(s=>s.Id == id)!;
            if (slider is null) return NotFound();

            SliderUpdateVM model = new SliderUpdateVM()
            {
                Id = slider.Id,
                Image = slider.Image,
                BgImage = slider.BgImage,
                Title = slider.Title,
                Offer = slider.Offer,
                ShortDesc = slider.ShortDesc,
                URL = slider.URL

            };
            if (!ModelState.IsValid) return View(slider);

            bool result = (!updatedSlider.MainPhoto?.IsValidLength(1) ?? true) || (!updatedSlider.BgPhoto?.IsValidLength(1) ?? true);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Please choose image file which size less than 1MB");
                return View(slider);
            }

            slider.Title = updatedSlider.Title;
            slider.ShortDesc = updatedSlider.ShortDesc;
            slider.Offer = updatedSlider.Offer;
            slider.URL = updatedSlider.URL;
            if(updatedSlider.MainPhoto is not null)
            {
                slider.Image.DeleteImage(_env.WebRootPath, "assets", "images", "website-images");
            }
            if (updatedSlider.BgPhoto is not null)
            {
                slider.BgImage.DeleteImage(_env.WebRootPath, "assets", "images", "website-images");
            }
            slider.Image = updatedSlider.MainPhoto is null ? slider.Image : await updatedSlider.MainPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");
            slider.BgImage = updatedSlider.BgPhoto is null ? slider.BgImage : await updatedSlider.BgPhoto.GeneratePhoto(_env.WebRootPath, "assets", "images", "website-images");

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
