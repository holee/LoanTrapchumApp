using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.ApprovedLoanRequest;

public record ApprovedRequestCommand(AproveLoanDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
