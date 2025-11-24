using Applications.Features.LoanRequests.Requests;
using FluentValidation;
namespace Applications.Features.LoanRequests.Command.RecheckedLoanRequest;

public class RecheckedRequestValidator : AbstractValidator<RecheckedRequestDtoCreate>
{
    public RecheckedRequestValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please select customer.")
                .NotNull()
                .WithMessage("Please select cusotmer");

        RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("Please select Branch.");

        RuleFor(x => x.LoanRequestId)
                    .NotEmpty()
                    .WithMessage("There is no loan request is selected.");
    }
}
