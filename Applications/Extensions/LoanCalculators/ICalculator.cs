using Domains.Models.Loans;
namespace Applications.Extensions.LoanCalculators
{
    public interface ICalculator
    {
        public  List<ScheduleLine> Calculate(int periodInMonth, decimal monthlyRate, decimal amount);
        public List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount);
        List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime firstPayment);
    }
}
