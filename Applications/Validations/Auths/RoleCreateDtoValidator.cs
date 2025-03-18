using Applications.Requests.Identity;
using FluentValidation;
namespace Applications.Validations.Auths;

public class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
{
    public RoleCreateDtoValidator() {
        RuleFor(x => x.RoleName)
                    .NotEmpty().WithMessage("Role Name is require.");
    }
}
