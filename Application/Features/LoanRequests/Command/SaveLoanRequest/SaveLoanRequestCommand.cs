using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.SaveLoanRequest;

public record SaveLoanRequestCommand(LoanRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
