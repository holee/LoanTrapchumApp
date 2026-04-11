using Applications.Features.LoanPayments.Dtos;
using Applications.Features.Schedules.Dto;
using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Interfaces.Loans;

public interface ILoanPaymentService
{
    Task<ErrorOr<IEnumerable<VSchedule>>> GetPaidScheduleAsync(int? branchId, string? loanId,CancellationToken cancellation = default);
    Task<ErrorOr<string>> RecievePayment(LoanTransaction req,CancellationToken cancellation = default);
    Task<ErrorOr<ScheduleDetailsDto>> GetScheduleByIdAsync(long? loanScheduleId, int? branchId, string? loanId);
    Task<ErrorOr<Reciept>> GenerateRecieptNumber(CancellationToken cancellation = default);
    Task<ErrorOr<PayOffResult>> PayOff(LoanTransaction trx, CancellationToken cancellation = default);
}

//Task<Result<IEnumerable<PaymentLineDetailsDto>>> GetPaymentLinesAsync(int? branchId,string? loanRefId, int? installmentNo);

//Task<Result<string>> CreatePayment(LoanTransaction req, CancellationToken cancellation = default);