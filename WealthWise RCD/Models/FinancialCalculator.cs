namespace WealthWise_RCD.Models
{
    public class FinancialCalculator
    {
        public double InitialAmount { get; set; }
        public double InterestRate { get; set; }
        public double NumberOfPayments { get; set; }
        public double Time { get; set; }
        public double InterestAmount { get; private set; }

        public bool CalculateInterest()
        {
            if (InitialAmount >= 0 && InterestRate >= 0 && NumberOfPayments > 0 && Time >= 0)
            {
                InterestAmount = Math.Round(InitialAmount * Math.Pow(1 + (InterestRate / NumberOfPayments), Time * NumberOfPayments),2);
                return true;
            }
            return false;
        }

        public int CurrentAge { get; set; }
        public int RetirementAge { get; set; }
        public double AnnualIncome { get; set; }
        public double ExpectedIncomeInc { get; set; }
        public double RetirementSavings { get; set; }
        public double SavingsInterestRate { get; set; }
        public double RetirementAmount { get; private set; }
        public Dictionary<int,double> YearlySavings { get; private set; } = new Dictionary<int,double>();

        public bool CalculateRetirement()
        {
            if (!(CurrentAge >= 0 && RetirementAge > CurrentAge && AnnualIncome >= 0 && ExpectedIncomeInc >= 0 && RetirementSavings >= 0 && SavingsInterestRate >= 0))
                return false;
            
            YearlySavings[CurrentAge-1]=0;
            for (int i = CurrentAge; i <= RetirementAge; i++)
            {
                AnnualIncome = AnnualIncome * (1+ExpectedIncomeInc);
                double SavingsThisYear = AnnualIncome * RetirementSavings;
                YearlySavings[i] = Math.Round((SavingsThisYear+YearlySavings[i-1]) * Math.Pow(1+ (SavingsInterestRate / 12), 12*1), 2);
            }
            RetirementAmount = YearlySavings[RetirementAge];
            return true;
        }
    }
}
