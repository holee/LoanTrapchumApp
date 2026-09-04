using ErrorOr;

namespace Application.Features.LoanPurposes.Commands.DeleteLoanPurpose;

public record DeleteLoanPurposeCommand(int Id) : IRequest<ErrorOr<bool>>;
