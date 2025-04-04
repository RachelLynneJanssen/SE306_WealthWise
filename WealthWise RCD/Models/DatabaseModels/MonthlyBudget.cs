using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WealthWise_RCD.Models.DatabaseModels
{
    public class MonthlyBudget
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Date when the record was created
        public decimal Income {  get; set; }
        public string UserID { get; set; } = null!; // Foreign key to ApplicationUser
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; } = null!;
        public decimal Expense {  get; set; }
        public decimal Savings { get; set; }
        public decimal Total {  get; set; }

    }
}
