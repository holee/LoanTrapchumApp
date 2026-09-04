using FluentValidation;

namespace Application.Features.Locations.Command.DeleteLocation;

public sealed class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("A valid location Id is required.");
    }
}
