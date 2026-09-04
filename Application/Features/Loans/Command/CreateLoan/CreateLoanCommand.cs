using Application.Features.Loans.Dtos.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Loans.Command.CreateLoan;

public record CreateLoanCommand(LoanDtoCreate Req) : IRequest<ErrorOr<DisburseResult>>;
