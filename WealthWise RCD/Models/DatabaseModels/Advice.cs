using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Advice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }

        public int AdvisorId { get; set; }

        [ForeignKey(nameof(AdvisorId))]
        public Advisor Advisor { get; set; } = null!;
    }
}
