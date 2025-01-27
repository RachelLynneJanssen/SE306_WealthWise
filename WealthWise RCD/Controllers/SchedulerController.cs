using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Controllers
{
    public class SchedulerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
