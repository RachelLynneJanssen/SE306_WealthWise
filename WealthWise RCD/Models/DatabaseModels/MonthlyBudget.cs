using System.ComponentModel.DataAnnotations.Schema;

namespace WealthWise_RCD.Models.DatabaseModels
{
    public class MonthlyBudget
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public decimal Income {  get; set; }
        public decimal Expense {  get; set; }
        public decimal Savings { get; set; }
        public decimal Total {  get; set; }
    }
}
