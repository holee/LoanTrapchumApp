using Applications.Requests.Identity;
using FluentValidation;
namespace Applications.Validations.Auths;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(req => req.Email)
                .NotEmpty().WithMessage("Email Or UserName Field is Required.");
        RuleFor(req => req.Password)
                .NotEmpty().WithMessage("Passwor Field is Required.")
                .MinimumLength(6).WithMessage("Password Length Must at leassr 6 character");
    }
}
