using Application.Features.LoanPurposes;
using Application.Responses.Paginations;
using ErrorOr;

namespace Application.Interfaces;

public interface ILoanPurposeService
{
    Task<Pager<LoanPurposeDto>> GetAllAsync(QueryParam? param, CancellationToken cancellation = default);
    Task<LoanPurposeDto?> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<ErrorOr<LoanPurposeDto>> CreateAsync(LoanPurposeDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<LoanPurposeDto>> UpdateAsync(LoanPurposeDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteAsync(int id, CancellationToken cancellation = default);
}
