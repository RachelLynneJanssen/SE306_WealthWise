using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    [Authorize(Roles = "Advisor,Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
