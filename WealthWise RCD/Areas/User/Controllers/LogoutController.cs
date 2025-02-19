using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return View("Logout");
        }
    }
}
