namespace Application.Features.Identity.Command.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
                .NotNull()
                    .WithMessage("User Id is required.")
                .NotEmpty()
                    .WithMessage("User Id is required.");
    }
}
