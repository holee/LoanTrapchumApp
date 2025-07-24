using Applications.Requests.Identity;
using FluentValidation;

namespace Applications.Features.Identity.Roles.Validation
{
    public sealed class RoleDtoUpdateValidator : AbstractValidator<RoleDtoUpdate>
    {
        public RoleDtoUpdateValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Name Is Required.");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Id Is Required");
        }
    }
}
