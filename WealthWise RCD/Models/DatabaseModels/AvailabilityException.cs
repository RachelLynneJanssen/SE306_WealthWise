using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class AvailabilityException
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AdvisorId { get; set; }
        [ForeignKey(nameof(AdvisorId))]
        public ApplicationUser Advisor { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}