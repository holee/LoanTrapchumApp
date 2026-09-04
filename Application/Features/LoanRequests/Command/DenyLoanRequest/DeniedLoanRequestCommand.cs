using Application.Features.LoanRequests.Requests;
using Application.Features.LoanRequests.Responses;
using Application.Features.Loans.Dtos.Responses;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DenyLoanRequest;

public record DeniedLoanRequestCommand(DeniedRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
