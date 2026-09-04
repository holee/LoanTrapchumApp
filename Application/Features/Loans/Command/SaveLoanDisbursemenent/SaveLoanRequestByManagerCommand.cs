using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Features.Loans.Command.SaveLoanDisbursemenent;

public sealed record SaveLoanRequestByManagerCommand(SaveLoanRequestDtoCreate Request): IRequest<ErrorOr<RequestResult>>;

