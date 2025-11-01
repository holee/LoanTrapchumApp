using FluentValidation;

namespace Applications.Features.Incomes.Queries.GetIncomeById
{
    public sealed class GetIncomeQueryValidator : AbstractValidator<GetIncomeByIdQuery>
    { 
        public GetIncomeQueryValidator() 
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customer ID");
        }
    }
}
