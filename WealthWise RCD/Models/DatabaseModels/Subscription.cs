using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Subscription
    {
        public int Id { get; set; }
        public int Level {  get; set; }
        public DateOnly ExpDate { get; set; }
        public bool Recurring {  get; set; }
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public int PaymentId { get; set; }
        
        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; set; } = null!;
    }
}
