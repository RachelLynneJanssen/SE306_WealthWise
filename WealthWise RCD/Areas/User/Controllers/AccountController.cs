using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
