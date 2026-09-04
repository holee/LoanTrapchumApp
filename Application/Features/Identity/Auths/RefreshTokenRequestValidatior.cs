using Application.Features.Identity.Users;
using FluentValidation;

namespace Application.Requests.Validations.Auths;

public class RefreshTokenRequestValidatior : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidatior()
    {
        RuleFor(req => req.Token)
                .NotEmpty().WithMessage("Token Field is Empty.");
        RuleFor(req => req.RefreshToken)
                .NotEmpty().WithMessage("Refresh Token Field is Empty.");
    }
}
