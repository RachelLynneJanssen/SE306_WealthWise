using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Reference
    {
        public int Id { get; set; }
        public string websiteUrl { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }

        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; } = null!;
    }
}
