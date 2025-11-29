using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.VerifiedLoanRequest;

public record VerifiedLoanRequestCommand(RequestDtoBaseCreate Request) : IRequest<ErrorOr<RequestResult>>;
