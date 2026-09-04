namespace Application.Features.LoanRequests.Command.PassRequest;

public sealed class PassRequestCommandValidator : AbstractValidator<PassRequestCommand>
{
    public PassRequestCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");
        RuleFor(x => x.Request.BranchId)
                .GreaterThan(0)
                    .WithMessage("Request Id must be greater than 0.");
        RuleFor(x => x.Request.LoanRequestId).Cascade(CascadeMode.Stop)
                    .NotNull()
                        .WithMessage("Loan Request Id cannot be null.")
                    .NotEmpty()
                        .WithMessage("Loan Request Id cannot be empty.");

        RuleFor(x=>x.Request.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Customer Id cannot be null.")
                .NotEmpty()
                    .WithMessage("Customer Id cannot be empty.");
    }
}

