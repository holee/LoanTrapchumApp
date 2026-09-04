using Domains.Models.Loans;
using ErrorOr;

namespace Application.Features.Loans.Queries.GetLoanById;

public record GetLoanByIdQuery(string? LoanId, int? BranchId) : IRequest<ErrorOr<LoanDisbursment>>;
