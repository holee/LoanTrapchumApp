using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest;

public record ApprovedLoanRequestCommand(AproveLoanDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
