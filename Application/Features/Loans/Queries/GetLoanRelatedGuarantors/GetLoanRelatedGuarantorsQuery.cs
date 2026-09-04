using Application.Enumerations;
using Application.Features.Loans.Dtos.Responses;
using Application.Utils;

namespace Application.Features.Loans.Queries.GetLoanRelatedGuarantors;

public record GetLoanRelatedGuarantorsQuery(string? LoanId, int? BranchId) : IRequest<IEnumerable<VLoanDisbursementGuarantors>>;
