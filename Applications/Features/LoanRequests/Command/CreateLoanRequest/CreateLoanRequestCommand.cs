using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.CreateLoanRequest;

public record CreateLoanRequestCommand(LoanRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
