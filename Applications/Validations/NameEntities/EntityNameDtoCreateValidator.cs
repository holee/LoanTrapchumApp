using Applications.Requests.Lists.Entities;
using FluentValidation;
namespace Applications.Validations.NameEntities
{
    public class EntityNameDtoCreateValidator : AbstractValidator<EntityNameDtoCreate>
    {
        public EntityNameDtoCreateValidator() { 
        
            RuleFor(x=>x.FirstName)
                    .NotEmpty()
                    .WithMessage("Please enter first name field");
            RuleFor(x => x.LastName)
                    .NotEmpty()
                     .WithMessage("Please enter last name field");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Please enter name field.");
        
        }
    }
}
