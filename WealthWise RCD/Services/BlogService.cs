using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;

namespace WealthWise_RCD.Services
{
    public class BlogService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BlogService(UserManager<ApplicationUser> userManager, ApplicationDbContext context) 
        {
            _userManager = userManager;
            _context = context; 
        }

        public async Task<List<Blog>> GetAllBlogPostsAsync()
        {
            return await _context.BlogPosts.Where(b => b.IsTip == false).ToListAsync();
        }
        public async Task<List<Blog>> GetTipsCategoryPostsAsync(string category)
        {
            return await _context.BlogPosts.ToListAsync();
        }

        public async Task UpsertBlogPostAsync(Blog blogPost, bool isTip = false)
        {
            if (blogPost.Id == 0)
            {
                _context.BlogPosts.Add(blogPost);
            }
            else
            {
                var postEntry = await _context.BlogPosts.FindAsync(blogPost.Id);
                if (postEntry != null)
                {
                    postEntry.Title = blogPost.Title;
                    postEntry.PublicationDate = blogPost.PublicationDate;
                    postEntry.ModificationDate = blogPost.ModificationDate;
                    postEntry.Price = blogPost.Price;
                    postEntry.Topic = blogPost.Topic;
                    postEntry.Content = blogPost.Content;
                    postEntry.RecommendationScore = blogPost.RecommendationScore;
                    postEntry.AdvisorId = blogPost.AdvisorId;
                    postEntry.IsTip = isTip;
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<ApplicationUser> GetBlogPostAuthorAsync(Blog post)
        {
            return await _userManager.FindByIdAsync(post.AdvisorId);
        }

        public async Task<List<Blog>> GetTopicTipsPostsAsync(string topic)
        {
            return await _context.BlogPosts.Where(b => b.IsTip == true && b.Topic == topic).ToListAsync();
        }
    }
}
