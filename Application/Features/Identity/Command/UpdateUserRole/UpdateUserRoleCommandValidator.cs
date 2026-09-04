namespace Application.Features.Identity.Command.UpdateUserRole;

public sealed class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(x => x.Request.UserId)
            .NotNull()
                .WithMessage("User Id is required.")
            .NotEmpty()
                .WithMessage("User Id is required.");
        RuleFor(x => x.Request.RoleName)
            .NotNull()
            .WithMessage("Role name is required.")
            .NotEmpty()
            .WithMessage("Role name is required.");
    }
}
