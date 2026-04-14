using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.RecheckedLoanRequest;

public record RecheckedLoanRequestCommand(RecheckedRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;
