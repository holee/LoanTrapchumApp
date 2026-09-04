using Application.Features.Identity.Users;
using FluentValidation;

namespace Application.Requests.Validations.Auths;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter your Username")
                                .Matches(@"^[a-zA-Z]+[0-9]*$").WithMessage("Username must start with letters.")
                                .Length(5, 50).WithMessage("{PropertyName} Length must be bewteen 5 and 50");
        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Please enter your username")
                    .EmailAddress()
                    .WithMessage("Please enter a valid email address.");
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Please enter your Password")
                                .Length(5, 50).WithMessage("{PropertyName} Length must be bewteen 5 and 50");
        RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
                                .NotEmpty().WithMessage("Please enter your Confirm Password")
                                .Length(5, 50).WithMessage("User Name Length must be bewteen 5 and 50")
                                .Equal(x => x.Password).WithMessage("Confirm Password and Passwrod must be match.");
        RuleFor(x => x.RoleName).NotEmpty()
                                .WithMessage("Please enter User Role.");
        RuleFor(x => x.BranchRefId)
                        .Cascade(CascadeMode.Stop)
                        .NotNull().WithMessage("Please Select Employee Branch.")
                        .GreaterThan(0).WithMessage("Please Select Employee Branch.");
        RuleFor(x => x.EmployeeId)
                            .Cascade(CascadeMode.Stop)
                            .NotEmpty().WithMessage("Employee must not blank.")
                            .NotNull().WithMessage("Please select employee.");
    }

}
