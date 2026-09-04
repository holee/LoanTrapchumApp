namespace Application.Extensions.LoanCalculators
{
    public class PmtCalc
    {
        public static double Pmt(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = yearlyInterestRate / 100 / 12;
            var denominator = Math.Pow(1 + rate, totalNumberOfMonths) - 1;
            return (rate + rate / denominator) * loanAmount;  
        }
    }
}
