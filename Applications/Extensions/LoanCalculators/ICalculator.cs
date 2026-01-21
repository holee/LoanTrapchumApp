using Domains.Models.Loans;
namespace Applications.Extensions.LoanCalculators
{
    public interface ICalculator
    {
        List<ScheduleLine> Calculate(int periodInMonth, decimal monthlyRate, decimal amount);
        List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount);
        List<ScheduleLine> Calculate(int? periodInMonth, decimal? monthlyRate, decimal? amount, DateTime? firstPayment = null);
    }
}
