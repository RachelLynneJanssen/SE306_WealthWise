using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
