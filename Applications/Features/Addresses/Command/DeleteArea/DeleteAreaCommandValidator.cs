namespace Applications.Features.Addresses.Command.DeleteArea;

public sealed class DeleteAreaCommandValidator : AbstractValidator<DeleteAreaCommand>
{
    public DeleteAreaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThan(0);
    }
}
