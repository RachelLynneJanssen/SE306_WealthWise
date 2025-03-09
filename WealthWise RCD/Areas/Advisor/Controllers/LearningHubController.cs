using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using System.Security.Claims;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    [Authorize(Roles = "Advisor,Admin")]
    public class LearningHubController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LearningHubController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        private static List<Blog> dummyBlogPosts = new List<Blog>
        {
            // To be removed later
            new Blog { Id = 1, Title = "Advisor Blog Post 1", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 1." },
            new Blog { Id = 2, Title = "Advisor Blog Post 2", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 2." },
            new Blog { Id = 3, Title = "Advisor Blog Post 3", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 3." },
        };

        public IActionResult BlogPosts()
        {
            return View(dummyBlogPosts);
        }

        public IActionResult BlogDetails(int id)
        {
            var blog = dummyBlogPosts.FirstOrDefault(b => b.Id == id);

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
        public async Task<IActionResult> PostBlog()
        {
            // Get the current user's claims to retrieve the saved blog draft
            var user = await _userManager.GetUserAsync(User);

            var titleClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "TempBlogTitle");
            var topicClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "TempBlogTopic");
            var contentClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "TempBlogContent");

            if (titleClaim == null || topicClaim == null || contentClaim == null)
            {
                return BadRequest("Not enough blog information saved to post.");
            }

            var blog = new Blog
            {
                Title = titleClaim.Value,
                Topic = topicClaim.Value,
                Content = contentClaim.Value,
                PublicationDate = DateTime.Now
            };

            // Save to database or simulate saving to list
            dummyBlogPosts.Add(blog);

            // Optionally remove claims after posting (if you don't want to retain draft data)
            await _userManager.RemoveClaimAsync(user, titleClaim);
            await _userManager.RemoveClaimAsync(user, topicClaim);
            await _userManager.RemoveClaimAsync(user, contentClaim);

            return Ok("Blog posted!");
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
