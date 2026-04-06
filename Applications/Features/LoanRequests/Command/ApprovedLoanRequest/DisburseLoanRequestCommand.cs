using Applications.Features.LoanRequests.Requests;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest;

public record DisburseLoanRequestCommand(AproveLoanDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
