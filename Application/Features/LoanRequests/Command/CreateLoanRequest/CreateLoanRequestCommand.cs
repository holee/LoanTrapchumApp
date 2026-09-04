using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.CreateLoanRequest;

public record CreateLoanRequestCommand(LoanRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
