using Applications.Features.LoanPurposes;
using Applications.Responses.Paginations;
using ErrorOr;

namespace Applications.Interfaces;

public interface ILoanPurposeService
{
    Task<Pager<LoanPurposeDto>> GetAllAsync(QueryParam? param, CancellationToken cancellation = default);
    Task<LoanPurposeDto?> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<ErrorOr<LoanPurposeDto>> CreateAsync(LoanPurposeDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<LoanPurposeDto>> UpdateAsync(LoanPurposeDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteAsync(int id, CancellationToken cancellation = default);
}
