using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest;

public record ApprovedRequestCommand(AproveLoanDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
