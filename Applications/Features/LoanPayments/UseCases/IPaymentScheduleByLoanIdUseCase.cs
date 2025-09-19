using Applications.Enumerations;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans;

namespace Applications.Features.LoanPayments.UseCases
{
    public interface IPaymentScheduleByLoanIdUseCase
    {
        Task<Result<VLoanDisbursments>> ExecuteAsync(string? loanId, int? branchId, ScheduleStatus state, bool include, CancellationToken cancellation = default);
    }
}