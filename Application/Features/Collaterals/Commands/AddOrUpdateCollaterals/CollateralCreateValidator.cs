using Application.Features.Collaterals.Dtos;
using Application.Features.Customers.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Collaterals.Commands.AddOrUpdateCollaterals
{
    public sealed class CollateralCreateValidator : AbstractValidator<CollateralDtoCreate>
    {
        public CollateralCreateValidator()
        {
            RuleFor(x => x.CustomerName)
                        .NotEmpty()
                            .WithMessage("Customer Is Required.");
            RuleForEach(x => x.CollateralLines)
                    .SetValidator(new CollateralLineCreateValidator());

        }
    }
}
