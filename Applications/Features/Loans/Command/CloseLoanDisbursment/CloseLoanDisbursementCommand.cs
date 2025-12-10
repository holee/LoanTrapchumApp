using ErrorOr;

namespace Applications.Features.Loans.Command.CloseLoanDisbursment;

public record CloseLoanDisbursementCommand(int? BranchId, string? LoanId,string? CustomerId) : IRequest<ErrorOr<string>>
{
}
