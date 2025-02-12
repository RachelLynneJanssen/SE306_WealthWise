using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class MonthlyBudget
    {
        public int Id {  get; set; }
        public decimal Income {  get; set; }
        public decimal Expense {  get; set; }
        public decimal Savings { get; set; }
        public decimal Total {  get; set; }
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
