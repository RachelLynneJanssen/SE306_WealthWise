using Microsoft.AspNetCore.Mvc;
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

        public IActionResult BlogPosts()
        {
            // To be removed later
            var dummyBlogPosts = new List<Blog>
            {
                new Blog { Title = "Blog Post 1", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 1." },
                new Blog { Title = "Blog Post 2", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 2." },
                new Blog { Title = "Blog Post 3", Topic = "Topic", PublicationDate= DateTime.Now, Content = "This is the content of Blog Post 3." },
            };

            return View(dummyBlogPosts);
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
