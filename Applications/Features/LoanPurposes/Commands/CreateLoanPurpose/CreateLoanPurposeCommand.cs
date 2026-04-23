using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.CreateLoanPurpose;

public record CreateLoanPurposeCommand(LoanPurposeDtoCreate Request) : IRequest<ErrorOr<LoanPurposeDto>>;
