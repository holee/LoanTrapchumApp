using Application.Features.Loans.Dtos.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Loans.Command.EditLoanDisbursment; 

public record EditLoanDisbursmentCommand(LoanDtoCreate Req) : IRequest<ErrorOr<DisburseResult>>;
