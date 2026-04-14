using Applications.Features.Collaterals.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedCollaterals;

public record GetLoanRelatedCollateralsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementCollateral>>;
