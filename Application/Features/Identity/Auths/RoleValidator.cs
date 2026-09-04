using Application.Features.Identity.Roles.Requests;
using FluentValidation;

namespace Application.Requests.Validations.Auths;

public class RoleValidator : AbstractValidator<RoleCreateDto>
{
    public RoleValidator()
    {
        RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role Name is required field.");
    }
}
