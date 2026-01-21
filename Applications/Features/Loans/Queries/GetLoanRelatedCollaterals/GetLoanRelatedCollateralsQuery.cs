using Applications.Features.Collaterals.Dtos;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedCollaterals;

public record GetLoanRelatedCollateralsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementCollateral>>;
