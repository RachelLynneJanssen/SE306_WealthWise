using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return View("Logout");
        }
    }
}
