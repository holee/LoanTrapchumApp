using Applications.Responses;

namespace Applications.Features.LoanPayments.Interfaces
{
    public interface IAddRecieptNumberUseCase
    {
        Task<Result<string>> ExecuteAsync(CancellationToken cancellation = default);
    }
}