using Application.Features.Guarantor.Dtos;

namespace Application.Features.Guarantor.Command.CreateGuarantor;

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

        RuleFor(x => x.Phone).Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Please enter phone number field.")
            .NotNull()
                .WithMessage("Please enter a valid phone number.");

        RuleFor(x => x.IdCard).Cascade(CascadeMode.Stop)
           .NotEmpty()
                .WithMessage("Please enter id card field.")
           .NotNull()
                .WithMessage("Please enter a valid id card number.");

    }
}
