using ErrorOr;

namespace Application.Features.Loans.Command.DeleteLoan;

public record DeleteLoanCommand(string? LoanId, int? BranchId, string? CustomerId) : IRequest<ErrorOr<DeleteResult>>;
