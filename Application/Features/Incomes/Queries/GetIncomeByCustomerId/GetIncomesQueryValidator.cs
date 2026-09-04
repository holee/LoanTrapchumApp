using FluentValidation;

namespace Application.Features.Incomes.Queries.GetIncomeByCustomerId
{
    public sealed class GetIncomesQueryValidator : AbstractValidator<GetIncomeByCustomerQuery>
    { 
        public GetIncomesQueryValidator()  
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customerId");
        }
    }
}
