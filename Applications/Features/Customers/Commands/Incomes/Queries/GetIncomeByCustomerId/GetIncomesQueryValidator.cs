using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId
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
