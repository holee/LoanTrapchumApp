using Application.Features.LoanRequests.Dtos;

namespace Application.Features.LoanRequests.Command.DeleteCoPawnCustomer;

public sealed class DeleteCoPawnCustomerValidator : AbstractValidator<CoPawnCustomerDto>
{
    public DeleteCoPawnCustomerValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("CustomerId is required.")
                .NotEmpty()
                    .WithMessage("CustomerId is required.");


        RuleFor(x => x.RequestId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("RequestId is required.")
                .NotEmpty()
                    .WithMessage("RequestId is required.");

        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("BranchId is required.")
                .GreaterThan(0)
                    .WithMessage("BranchId must be greater than 0.");
    }
}