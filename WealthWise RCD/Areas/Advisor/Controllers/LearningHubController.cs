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
        public async Task<IActionResult> PostBlog([FromBody] Blog blog)
        {
            if (blog == null || string.IsNullOrWhiteSpace(blog.Title) || string.IsNullOrWhiteSpace(blog.Topic) || string.IsNullOrWhiteSpace(blog.Content))
            {
                return BadRequest("Blog Title, Topic, and Content are required.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            blog.PublicationDate = DateTime.Now;
            blog.AdvisorId = user.Id;
            blog.Advisor = user;

            // save blog to database
            try
            {
                await _blogService.UpsertBlogPostAsync(blog);
                return Ok("Blog posted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to post blog. Error: " + ex.Message);
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

    }
}
