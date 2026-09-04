using Application.Features.LoanRequests.Requests;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.SaveLoanRequests;

public sealed record SaveLoanRequestsCommand(SaveLoanRequestDtoCreate Request): IRequest<ErrorOr<RequestResult>>; 

