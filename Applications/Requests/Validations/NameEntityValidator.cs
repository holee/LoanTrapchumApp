using Applications.Requests.Lists.Entities;
using FluentValidation;

namespace Applications.Requests.Validations
{
    public class NameEntityValidator: AbstractValidator<EntEmployeeDtoCreate>
    {
        public NameEntityValidator()
        {
            //RuleFor(e=>e.em)
            //        .NotEmpty()
            //            .WithMessage("{PropertyName} should be not empty.")
            //                .Length(100);
        }
    }
}
