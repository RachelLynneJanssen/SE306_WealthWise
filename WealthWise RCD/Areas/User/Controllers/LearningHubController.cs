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
            foreach (var blogPost in blogPosts)
            {
                blogPost.Advisor = await _blogService.GetBlogPostAuthorAsync(blogPost);
            }
            return View(blogPosts);
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            var blogPosts = await _blogService.GetAllPosts();
            var blog = blogPosts.FirstOrDefault(b => b.Id == id); // will fix later

            if(blog is null)
            {
                return NotFound();
            }
            blog.Advisor = await _blogService.GetBlogPostAuthorAsync(blog);
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

        public async Task<IActionResult> InvestmentTips()
        {
            var tips = await _blogService.GetTipsCategoryPostsAsync("Investing Advice");
            foreach (var tip in tips)
            {
                tip.Advisor = await _blogService.GetBlogPostAuthorAsync(tip);
            }
            return View("TipsPages/InvestmentTips", tips);
        }

        public async Task<IActionResult> SavingsTips()
        {
            var tips = await _blogService.GetTipsCategoryPostsAsync("Savings Advice");
            foreach (var tip in tips)
            {
                tip.Advisor = await _blogService.GetBlogPostAuthorAsync(tip);
            }
            return View("TipsPages/SavingsTips", tips);
        }

        public async Task<IActionResult> MortgageTips()
        {
            {
                var tips = await _blogService.GetTipsCategoryPostsAsync("Mortgage Advice");
                foreach (var tip in tips)
                {
                    tip.Advisor = await _blogService.GetBlogPostAuthorAsync(tip);
                }
                return View("TipsPages/MortgageTips", tips);
            }
        }

        public async Task<IActionResult> CardTips()
        {
            var tips = await _blogService.GetTipsCategoryPostsAsync("Credit Card Advice");
            foreach (var tip in tips)
            {
                tip.Advisor = await _blogService.GetBlogPostAuthorAsync(tip);
            }
            return View("TipsPages/CardTips", tips);
        }

    }
}
