using FluentValidation;

namespace Application.Features.LoanRequests.Command.AddDescription;

public sealed class AddDescriptionCommandValidator : AbstractValidator<AddDescriptionCommand>
{
    public AddDescriptionCommandValidator()
    {
        RuleFor(x => x.Request.LoanRequestRefId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("LoanRequestRefId is required.")
            .NotNull().WithMessage("LoanRequestRefId must be provided.");
        RuleFor(x => x.Request.Memo)
            .NotEmpty().WithMessage("Memo is required.")
            .MaximumLength(1000).WithMessage("Memo must not exceed 1000 characters.");
    }
}
