using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals
{
    public sealed class CollateralCreateValidator : AbstractValidator<CollateralDtoCreate>
    {
        public CollateralCreateValidator()
        {
            RuleFor(x => x.CustomerName)
                        .NotEmpty()
                            .WithMessage("Customer Is Required.");

            //RuleFor(x => x.CollateralLines)
            //                .NotEmpty()
            //                    .WithMessage("At least one collateral line is required.");

            RuleForEach(x => x.CollateralLines)
                    .SetValidator(new CollateralLineCreateValidator());

        }
    }
}
