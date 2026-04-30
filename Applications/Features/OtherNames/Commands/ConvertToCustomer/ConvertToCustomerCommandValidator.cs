namespace Applications.Features.OtherNames.Commands.ConvertToCustomer;

public sealed class ConvertToCustomerCommandValidator : AbstractValidator<ConvertToCustomerCommand>
{
    public ConvertToCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
