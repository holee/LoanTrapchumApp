using Applications.Features.LoanRequests.Requests;
using Applications.Features.LoanRequests.Responses;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DenyLoanRequest;

public record DeniedLoanRequestCommand(DeniedRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
