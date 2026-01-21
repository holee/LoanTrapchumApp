using Applications.Features.Guarantor.Dtos;

namespace Applications.Features.Guarantor.Command.CreateGuarantor
{
    public class EntityGuarantorCreateValidator : AbstractValidator<EntityGuarantorCreate> 
    {
        public EntityGuarantorCreateValidator() 
        {

            RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithMessage("Please enter first name field");
            RuleFor(x => x.LastName)
                    .NotEmpty()
                     .WithMessage("Please enter last name field");
            RuleFor(x => x.AltName)
                .NotEmpty()
                .WithMessage("Please enter name field.");

        }
    }
}
