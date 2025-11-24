using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.RecheckedLoanRequest;

public record RecheckedLoanRequestCommand(RecheckedRequestDtoCreate Request) : IRequest<ErrorOr<RequestResult>>;

public sealed class RecheckedLoanRequestCommandValidator : AbstractValidator<RecheckedLoanRequestCommand>
{
    public RecheckedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new RecheckedRequestValidator());
    }
}