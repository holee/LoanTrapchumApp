using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.Loans.Command.SaveLoanDisbursemenent;

public sealed record SaveLoanDisbursementCommand(SaveLoanDisbursementDtoCreate Request): IRequest<ErrorOr<RequestResult>>;

