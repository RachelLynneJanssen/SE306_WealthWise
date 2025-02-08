namespace WealthWise_RCD.Models
{
    public class FinancialCalculator
    {
        public double InitialAmount { get; set; }
        public double InterestRate { get; set; }
        public double NumberOfPayments { get; set; }
        public double Time { get; set; }
        public double Amount { get; private set; }

        public bool Calculate()
        {
            if (InitialAmount >= 0 && InterestRate >= 0 && NumberOfPayments > 0 && Time >= 0)
            {
                Amount = Math.Round(InitialAmount * Math.Pow((1 + ((InterestRate/100) / NumberOfPayments)), (Time * NumberOfPayments)),2);
                return true;
            }
            return false;
        }
    }
}
