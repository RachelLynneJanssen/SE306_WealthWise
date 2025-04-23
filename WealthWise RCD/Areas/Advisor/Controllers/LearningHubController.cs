using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using System.Security.Claims;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Services;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    [Authorize(Roles = "Advisor,Admin")]
    public class LearningHubController : Controller
    {
        private readonly BlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LearningHubController(UserManager<ApplicationUser> userManager, BlogService blogService)
        {
            _userManager = userManager;
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
            var blogPosts = await _blogService.GetAllBlogPostsAsync();
            var blog = blogPosts.FirstOrDefault(b => b.Id == id); // will fix later

            if (blog is null)
            {
                return NotFound();
            }
            blog.Advisor = await _blogService.GetBlogPostAuthorAsync(blog);
            return View(blog);
        }

        [HttpPost]
        [Route("/Advisor/LearningHub/SaveTempBlog")]
        public async Task<IActionResult> SaveTempBlog([FromBody] TempBlog tempBlog)
        {
            if (tempBlog == null)
            {
                return BadRequest("Invalid blog data.");
            }

            // Store the temp blog in user claims
            var user = await _userManager.GetUserAsync(User);

            // Remove previous claims if they exist
            await _userManager.RemoveClaimAsync(user, new Claim("TempBlogTitle", string.Empty));
            await _userManager.RemoveClaimAsync(user, new Claim("TempBlogTopic", string.Empty));
            await _userManager.RemoveClaimAsync(user, new Claim("TempBlogContent", string.Empty));

            // Add new claims with the draft blog content
            await _userManager.AddClaimAsync(user, new Claim("TempBlogTitle", tempBlog.Title));
            await _userManager.AddClaimAsync(user, new Claim("TempBlogTopic", tempBlog.Topic));
            await _userManager.AddClaimAsync(user, new Claim("TempBlogContent", tempBlog.Content));

            return Ok("Blog draft saved!");
        }

        [HttpPost]
        [Route("/Advisor/LearningHub/PostBlog")]
        public async Task<IActionResult> PostBlog([FromBody] Blog blog, bool isTip = false)
        {
            if (blog == null || string.IsNullOrWhiteSpace(blog.Title) || string.IsNullOrWhiteSpace(blog.Topic) || string.IsNullOrWhiteSpace(blog.Content))
            {
                return BadRequest("Title, Topic, and Content are required to post.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            blog.PublicationDate = DateTime.Now;
            blog.AdvisorId = user.Id;
            blog.Advisor = user;
            blog.IsTip = isTip;

            // save blog to database
            try
            {
                await _blogService.UpsertBlogPostAsync(blog);
                return Ok("Blog posted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to post. Error: " + ex.Message);
            }
        }

        public IActionResult BlogCreator()
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
        public async Task <IActionResult> InvestmentTips()
        {
            List<Blog> tips = await _blogService.GetAllTipsPostsAsync("Interest Advice");
            return View("TipsPages/InvestmentTips", tips);
        }

        public async Task<IActionResult> SavingsTips()
        {
            List<Blog> tips = await _blogService.GetAllTipsPostsAsync("Savings Advice");
            return View("TipsPages/SavingsTips", tips);
        }

        public async Task<IActionResult> MortgageTips()
        {
            List<Blog> tips = await _blogService.GetAllTipsPostsAsync("Mortgage Advice");
            return View("TipsPages/MortgageTips", tips);
        }

        public async Task<IActionResult> CardTips()
        {
            List<Blog> tips = await _blogService.GetAllTipsPostsAsync("Credit Card Advice");
            return View("TipsPages/CardTips", tips);
        }
        public IActionResult TipsCreator()
        {
            return View("TipsPages/TipsCreator");
        }
    }
}
