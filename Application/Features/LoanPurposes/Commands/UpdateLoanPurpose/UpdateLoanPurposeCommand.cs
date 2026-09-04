using ErrorOr;

namespace Application.Features.LoanPurposes.Commands.UpdateLoanPurpose;

public record UpdateLoanPurposeCommand(LoanPurposeDtoUpdate Request) : IRequest<ErrorOr<LoanPurposeDto>>;
