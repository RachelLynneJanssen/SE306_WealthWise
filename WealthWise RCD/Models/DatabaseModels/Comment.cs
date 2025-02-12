using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content {  get; set; }
        public DateTime PublicationDate { get; set; }

        // Parent Blog post?
        
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; } = null!;
    }
}
