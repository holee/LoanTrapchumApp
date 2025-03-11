using Applications.Requests.Lists.Employees;
using FluentValidation;

namespace Applications.Requests.Validations.Employees
{
    public class EmployeeDtoUpdateValidator : AbstractValidator<EmployeeDtoUpdate> 
    {
        public EmployeeDtoUpdateValidator() 
        {
            RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$");
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$");
        }
    }
}
