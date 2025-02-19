using Microsoft.AspNetCore.Mvc;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    public class LearningHubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlogPosts()
        {
            return View();
        }

        public IActionResult References()
        {
            return View();
        }

        public IActionResult TipsAndTricks()
        {
            return View();
        }
    }
}
