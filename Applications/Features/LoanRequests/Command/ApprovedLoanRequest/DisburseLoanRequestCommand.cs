using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest;

public record DisburseLoanRequestCommand(AproveLoanDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
