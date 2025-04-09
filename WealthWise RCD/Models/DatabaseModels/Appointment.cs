using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime ScheduledTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AdvisorId { get; set; }
        
        [ForeignKey(nameof(AdvisorId))]
        public ApplicationUser Advisor { get; set; } = null!;
        public string UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}