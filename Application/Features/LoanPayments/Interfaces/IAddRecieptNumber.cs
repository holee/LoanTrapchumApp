using Application.Responses;
using ErrorOr;

namespace Application.Features.LoanPayments.Interfaces
{
    public interface IAddRecieptNumber
    {
        Task<ErrorOr<string?>> ExecuteAsync(CancellationToken cancellation = default);
    }
}