using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Reference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string websiteUrl { get; set; }
        public DateTime? PublicationDate { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string? Author { get; set; }
        [MaxLength(255)]
        public string? Publisher { get; set; }

        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; } = null!;
    }
}
