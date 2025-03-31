using Applications.Requests.Identity;
using FluentValidation;

namespace Applications.Requests.Validations.Auths;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter your username")
                                .Length(5,50).WithMessage("{PropertyName} Length must be bewteen 5 and 50");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your username");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter your Password")
                                .Length(5, 50).WithMessage("{PropertyName} Length must be bewteen 5 and 50");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please enter your Confirm Password")
                                .Length(5, 50).WithMessage("User Name Length must be bewteen 5 and 50")
                                .Equal(x=>x.Password).WithMessage("Confirm Password and Passwrod must be match.");
        RuleFor(x => x.RoleName).NotEmpty()
                                .WithMessage("{Please enter User Role.");
        RuleFor(x => x.BranchRefId).GreaterThan(0)
                                .WithMessage("Please Select Employee Branch.")
                                .NotNull()
                                .WithMessage("Please Select Employee Branch.");
        RuleFor(x => x.EmployeeId)
                            .NotNull()
                            .WithMessage("Please select employee.")
                            .NotNull()
                            .WithMessage("Please Select Employee.");
    }

}
