using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedGuarantors;

public record GetLoanRelatedGuarantorsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanGuarantors>>;
