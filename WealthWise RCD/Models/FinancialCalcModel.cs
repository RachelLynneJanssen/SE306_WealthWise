namespace WealthWise_RCD.Models
{
    public class InterestCalculatorModel
    {
        public decimal LoanAmount { get; set; }
        public int NumberOfPayments { get; set; }
        public int Time { get; set; }
        public decimal InterestRate { get; set; }
    }


}