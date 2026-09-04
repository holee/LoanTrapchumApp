using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.SaveEditLoanRequest;

public record SaveEditLoanRequestCommand(LoanRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
