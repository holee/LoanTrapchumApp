using Applications.Features.Customers.Commands.CustomerIncomeQuery;
using FluentValidation;

namespace Applications.Features.Customers.Commands.CustomerExpenseQuery
{
    public sealed class GetCustomerIncomeQueryValidator : AbstractValidator<GetCustomerIncomeQuery>
    { 
        public GetCustomerIncomeQueryValidator() 
        {
            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customerId");
        }
    }
}
