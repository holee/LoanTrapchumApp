using Applications.Requests.Loans.Payments;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Loans;

public interface ILoanPaymentService
{
    Task<Result<IEnumerable<LoanPaymentLineVm>>> GetPaymentLinesAsync(int? branchId,string? loanRefId, int? installmentNo);
    Task<Result<IEnumerable<VSchedule>>> GetPaidScheduleAsync(int? branchId, string? loanId);
    Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req);
    Task<Result<string>> RecievePayment(LoanTransaction req,CancellationToken cancellation = default);
    Task<Result<ScheduleVm>> GetScheduleByIdAsync(long? loanScheduleId, int? branchId, string? loanId);
    Task<Result<string>> CreatePayment(LoanTransaction req, CancellationToken cancellation = default);
}
