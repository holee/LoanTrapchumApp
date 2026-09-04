using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Utils;
using ErrorOr;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.RecheckedLoanRequest;

public record RecheckedLoanRequestCommand(RecheckedRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
