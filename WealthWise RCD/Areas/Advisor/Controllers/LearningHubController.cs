using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    public class LearningHubController : Controller
    {
        [Area("Advisor")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
