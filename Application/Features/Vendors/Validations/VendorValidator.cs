using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.OtherNames.Dto;

namespace Application.Validations.NameEntities
{
    public sealed class VendorValidator : AbstractValidator<EntityVendorCreate>
    {
        public VendorValidator()
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
