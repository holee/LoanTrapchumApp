namespace Application.Extensions
{
    public static class PmtExenstion
    {
        public static double Pmt(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = (double)yearlyInterestRate / 100 / 12;
            var denominator = Math.Pow(1 + rate, totalNumberOfMonths) - 1;
            return (rate + rate / denominator) * loanAmount;
        }
        public static decimal Pmt(decimal yearlyInterestRate, int totalNumberOfMonths, decimal loanAmount)
        {
            var rate = (double)yearlyInterestRate / 100 / 12;
            var denominator = Math.Pow(1 + rate, totalNumberOfMonths) - 1;
            return (decimal)(rate + rate / denominator) * loanAmount; 
        }
    }
}
