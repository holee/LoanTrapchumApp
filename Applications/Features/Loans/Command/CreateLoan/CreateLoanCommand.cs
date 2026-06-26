using Applications.Features.Loans.Dtos.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Loans.Command.CreateLoan;

public record CreateLoanCommand(LoanDtoCreate Req) : IRequest<ErrorOr<DisburseResult>>;
