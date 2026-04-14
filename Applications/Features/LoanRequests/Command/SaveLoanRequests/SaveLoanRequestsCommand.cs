using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequests;

public sealed record SaveLoanRequestsCommand(SaveLoanRequestDtoCreate Request): IRequest<ErrorOr<RequestResult>>; 

