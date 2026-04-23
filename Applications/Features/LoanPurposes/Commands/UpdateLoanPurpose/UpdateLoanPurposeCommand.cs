using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.UpdateLoanPurpose;

public record UpdateLoanPurposeCommand(LoanPurposeDtoUpdate Request) : IRequest<ErrorOr<LoanPurposeDto>>;
