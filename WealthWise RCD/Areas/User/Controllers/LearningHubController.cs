using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Areas.User.Controllers
{
    [Area("User")]
    public class LearningHubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private static List<Blog> dummyBlogPosts = new List<Blog>
        {
            // To be removed later
            new Blog { Id = 1, Title = "Blog Post 1", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 1." },
            new Blog { Id = 2, Title = "Blog Post 2", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 2." },
            new Blog { Id = 3, Title = "Blog Post 3", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 3." },
        };

        public IActionResult BlogPosts()
        {
            return View(dummyBlogPosts);
        }

        public IActionResult BlogDetails(int id)
        {
            var blog = dummyBlogPosts.FirstOrDefault(b => b.Id == id);

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

    }
}
