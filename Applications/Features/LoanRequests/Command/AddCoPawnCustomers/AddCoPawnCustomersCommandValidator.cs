namespace Applications.Features.LoanRequests.Command.AddCoPawnCustomers;

public sealed class AddCoPawnCustomersCommandValidator : AbstractValidator<AddCoPawnCustomersCommand>
{
    public AddCoPawnCustomersCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");

        RuleForEach(x => x.Request)
               .ChildRules(request =>
               {
                   request.RuleFor(x => x.BranchId)
                       .NotNull()
                           .WithMessage("Branch ID is required.")
                       .GreaterThan(0)
                           .WithMessage("Branch ID must be greater than 0.");
               });

    }
}
