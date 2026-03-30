using Applications.Features.LoanPayments.Requests;
using Applications.Features.Schedules.Dto;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Loans;

public interface ILoanPaymentService
{
    Task<Result<IEnumerable<PaymentLineDetailsDto>>> GetPaymentLinesAsync(int? branchId,string? loanRefId, int? installmentNo);
    Task<Result<IEnumerable<VSchedule>>> GetPaidScheduleAsync(int? branchId, string? loanId,CancellationToken cancellation = default);
    Task<Result<string>> RecievePayment(LoanPaymentDtoCreate req);
    Task<ErrorOr.ErrorOr<string>> RecievePayment(LoanTransaction req,CancellationToken cancellation = default);
    Task<Result<ScheduleDetailsDto>> GetScheduleByIdAsync(long? loanScheduleId, int? branchId, string? loanId);
    Task<Result<string>> CreatePayment(LoanTransaction req, CancellationToken cancellation = default);
    Task<Result<Reciept>> GenerateRecieptNumber(CancellationToken cancellation = default);
}
