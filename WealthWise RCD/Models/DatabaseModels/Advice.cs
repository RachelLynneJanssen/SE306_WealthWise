using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Advice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        [MaxLength(255)]
        public string Category { get; set; }
        public string Content { get; set; }
        public string AdvisorId { get; set; }

        [ForeignKey(nameof(AdvisorId))]
        public ApplicationUser Advisor { get; set; } = null!;
    }
}
