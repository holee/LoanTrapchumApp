using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Responses;

namespace Applications.Features.LoanPayments.UseCases
{
    public interface IPaymentScheduleByLoanIdUseCase
    {
        Task<Result<VLoanDisbursments>> ExecuteAsync(string? loanId, int? branchId, ScheduleStatus state, bool include, CancellationToken cancellation = default);
    }
}