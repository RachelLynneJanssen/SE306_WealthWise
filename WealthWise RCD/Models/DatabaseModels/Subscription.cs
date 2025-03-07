using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class Subscription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Level {  get; set; }
        public DateOnly ExpDate { get; set; }
        public bool Recurring {  get; set; }
        public int? PaymentId { get; set; }
        
        [ForeignKey(nameof(PaymentId))]
        public Payment? Payment { get; set; }
    }
}
