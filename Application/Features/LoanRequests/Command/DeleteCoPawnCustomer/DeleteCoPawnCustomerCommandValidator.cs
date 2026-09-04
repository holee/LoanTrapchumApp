namespace Application.Features.LoanRequests.Command.DeleteCoPawnCustomer;

public sealed class DeleteCoPawnCustomerCommandValidator : AbstractValidator<DeleteCoPawnCustomerCommand>
{
    public DeleteCoPawnCustomerCommandValidator()
    {
        RuleFor(x => x.CoPawnRequest).SetValidator(new DeleteCoPawnCustomerValidator());
    }
}
