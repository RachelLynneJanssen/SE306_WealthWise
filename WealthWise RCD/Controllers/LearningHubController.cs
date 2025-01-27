using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Controllers
{
    public class LearningHubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
