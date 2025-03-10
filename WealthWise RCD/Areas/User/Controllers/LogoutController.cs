using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return View("Logout");
        }
    }
}
