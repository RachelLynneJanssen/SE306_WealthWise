using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("Error");
        }
    }
}
