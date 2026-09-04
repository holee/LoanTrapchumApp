using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.VerifiedLoanRequest;

public record VerifiedLoanRequestCommand(RequestDtoBaseCreate Request) : IRequest<ErrorOr<RequestResult>>;
