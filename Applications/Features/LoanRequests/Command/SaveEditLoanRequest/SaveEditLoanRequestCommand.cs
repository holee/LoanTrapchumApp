using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.SaveEditLoanRequest;

public record SaveEditLoanRequestCommand(LoanRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
