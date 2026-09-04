using Application.Features.Collaterals.Dtos;
using Application.Features.Loans.Dtos.Responses;
using Application.Utils;

namespace Application.Features.Loans.Queries.GetLoanRelatedCollaterals;

public record GetLoanRelatedCollateralsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementCollateral>>;
