namespace Application.Features.Loans.Command.RequestDelete;

public sealed class RequestDeleteCommandValidator : AbstractValidator<RequestDeleteCommand>
{
    public RequestDeleteCommandValidator()
    {
        RuleFor(x => x.Request.LoanId)
            .NotNull().WithMessage("LoanId is required.")
            .NotEmpty().WithMessage("LoanId is required.");
        RuleFor(x => x.Request.BranchId)
            .NotNull().WithMessage("BranchId is required.")
            .NotEmpty().WithMessage("BranchId is required.");
    }
}

