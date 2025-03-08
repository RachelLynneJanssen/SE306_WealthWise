using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Areas.Advisor.Controllers
{
    [Area("Advisor")]
    public class LearningHubController : Controller
    {
        
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
        public IActionResult SaveTempBlog([FromBody] TempBlog tempBlog)
        {
            if (tempBlog is null)
            {
                return BadRequest("Invalid blog data.");
            }

            HttpContext.Session.SetString("TempBlogTitle", tempBlog.Title);
            HttpContext.Session.SetString("TempBlogTopic", tempBlog.Topic);
            HttpContext.Session.SetString("TempBlogContent", tempBlog.Content);

            return Ok("Blog draft saved!");
        }

        [HttpPost]
        [Route("/Advisor/LearningHub/PostBlog")]
        public IActionResult PostBlog()
        {
            string title = HttpContext.Session.GetString("TempBlogTitle");
            string topic = HttpContext.Session.GetString("TempBlogTopic");
            string content = HttpContext.Session.GetString("TempBlogContent");

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(topic))
            {
                return BadRequest("Not enough blog information saved to post.");
            }

            var blog = new Blog
            {
                Title = title,
                Topic = topic,
                Content = content,
                PublicationDate = DateTime.Now
            };

            // Save to database
            // _dbContext.Blogs.Add(blog);
            // _dbContext.SaveChanges();

            // simulate saving to database
            dummyBlogPosts.Add(blog);

            // Clear session data
            HttpContext.Session.Remove("TempBlogTitle");
            HttpContext.Session.Remove("TempBlogTopic");
            HttpContext.Session.Remove("TempBlogContent");

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
