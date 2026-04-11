namespace Applications.Features.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Role).NotNull().WithMessage("Role data is required.");
        RuleFor(x => x.Role.RoleName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Role name is required.")
                .NotNull()
                    .WithMessage("Role name cannot be null.");
        // Additional validation rules can be added here as needed
    }
}
