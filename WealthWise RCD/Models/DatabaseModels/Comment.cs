using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content {  get; set; }
        public DateTime PublicationDate { get; set; }

        public int ParentCommentId { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public Comment ParentComment { get; set; } = null!;
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; } = null!;
    }
}
