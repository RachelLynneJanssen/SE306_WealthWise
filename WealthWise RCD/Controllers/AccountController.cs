using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
