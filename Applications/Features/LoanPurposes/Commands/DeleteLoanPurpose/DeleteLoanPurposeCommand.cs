using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.DeleteLoanPurpose;

public record DeleteLoanPurposeCommand(int Id) : IRequest<ErrorOr<bool>>;
