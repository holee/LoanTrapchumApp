using Applications.Requests.Lists.OtherNames;
using FluentValidation;
namespace Applications.Validations.Employees
{
    public class EntityEmployeeCreateValidator : AbstractValidator<EntityEmployeeCreate> 
    {
        public EntityEmployeeCreateValidator() 
        {
            RuleFor(x => x.AltName)
                    .NotEmpty().WithMessage("Please Khmer Name Field");

            RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$");
            RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Please input First Name Field")
                    .Matches(@"^[\p{L}]+$");

            RuleFor(x => x.BranchRefId)
                    .NotNull().WithMessage("Please select employee branch.")
                    .GreaterThan(0).WithMessage("Please select employee branch.");
        }
    }
}
