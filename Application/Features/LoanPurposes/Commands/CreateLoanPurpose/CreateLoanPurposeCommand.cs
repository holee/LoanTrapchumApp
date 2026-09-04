using ErrorOr;

namespace Application.Features.LoanPurposes.Commands.CreateLoanPurpose;

public record CreateLoanPurposeCommand(LoanPurposeDtoCreate Request) : IRequest<ErrorOr<LoanPurposeDto>>;
