using Microsoft.AspNetCore.Mvc;

namespace P140_Pronia.Areas.Admin.Controllers
{
    [Area(areaName:"Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
