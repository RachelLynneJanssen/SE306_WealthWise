using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string AdvisorId { get; set; }
        
        [ForeignKey(nameof(AdvisorId))]
        public User Advisor { get; set; } = null!;
        public string UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}