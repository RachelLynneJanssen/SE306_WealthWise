using Microsoft.EntityFrameworkCore;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Services
{
    public class BlogService
    {

        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context) 
        {  
            _context = context; 
        }

        public async Task<List<Blog>> GetAllBlogPostsAsync()
        {
            return await _context.BlogPosts.ToListAsync();
        }
    }
}
