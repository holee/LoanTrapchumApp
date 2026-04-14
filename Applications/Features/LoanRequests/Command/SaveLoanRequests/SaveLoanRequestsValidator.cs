using Applications.Features.LoanRequests.Requests;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequests;

public class SaveLoanRequestsValidator : AbstractValidator<SaveLoanRequestDtoCreate>
{
    public SaveLoanRequestsValidator()
    {
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("LoanRequestId cannot be null.")
                .NotEmpty()
                .WithMessage("LoanRequestId is required.");

        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId cannot be null.")
                .GreaterThan(0)
                .WithMessage("BranchId must be greater than 0.");


        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("CustomerId cannot be null.")
                .NotEmpty()
                .WithMessage("CustomerId is required.");

    }
}