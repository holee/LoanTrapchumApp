using Applications.Features.Loans.Dtos.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Loans.Command.EditLoanDisbursment; 

public record EditLoanDisbursmentCommand(LoanDtoCreate Req) : IRequest<ErrorOr<DisburseResult>>;
