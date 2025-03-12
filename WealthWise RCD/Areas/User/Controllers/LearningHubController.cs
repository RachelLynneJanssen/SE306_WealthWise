using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Services;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admin")]
    public class LearningHubController : Controller
    {
        private readonly BlogService _blogService; // for database interactions

        public LearningHubController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BlogPosts()
        {
            var blogPosts = await _blogService.GetAllBlogPostsAsync();
            return View(blogPosts);
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            var blogPosts = await _blogService.GetAllBlogPostsAsync();
            var blog = blogPosts.FirstOrDefault(b => b.Id == id); // will fix later

            if(blog is null)
            {
                return NotFound();
            }

            return View(blog);
        }

        public IActionResult References()
        {
            return View();
        }

        public IActionResult TipsAndTricks()
        {
            return View();
        }

        public IActionResult InvestmentTips()
        {
            return View("TipsPages/InvestmentTips");
        }

        public IActionResult SavingsTips()
        {
            return View("TipsPages/SavingsTips");
        }

        public IActionResult MortgageTips()
        {
            return View("TipsPages/MortgageTips");
        }

        public IActionResult CardTips()
        {
            return View("TipsPages/CardTips");
        }

    }
}
