using Applications.Responses;
using ErrorOr;

namespace Applications.Features.LoanPayments.Interfaces
{
    public interface IAddRecieptNumber
    {
        Task<ErrorOr<string?>> ExecuteAsync(CancellationToken cancellation = default);
    }
}