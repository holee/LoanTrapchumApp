using ErrorOr;

namespace Application.Features.Loans.Queries.GetLoanForEdit;

public record GetLoanForEditQuery(string? LoanId, int? BranchId) : IRequest<ErrorOr<LoanDtoCreate>>
{
}
