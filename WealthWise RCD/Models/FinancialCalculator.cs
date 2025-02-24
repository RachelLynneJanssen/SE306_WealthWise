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
                InterestAmount = Math.Round(InitialAmount * Math.Pow((1 + ((InterestRate/100) / NumberOfPayments)), (Time * NumberOfPayments)),2);
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

        public bool CalculateRetirement()
        {
            if (!(CurrentAge >= 0 && RetirementAge > CurrentAge && AnnualIncome >= 0 && ExpectedIncomeInc >= 0 && RetirementSavings >= 0 && SavingsInterestRate >= 0))
                return false;
            
            double yearsToRetirement = RetirementAge - CurrentAge;
            double futureValue = RetirementSavings * Math.Pow((1 + (SavingsInterestRate / 100)), yearsToRetirement);
            double annualIncomeAtRetirement = AnnualIncome * Math.Pow((1 + (ExpectedIncomeInc / 100)), yearsToRetirement);
            RetirementAmount = Math.Round(futureValue - annualIncomeAtRetirement, 2);
            return true;
            
        }

    }
}
