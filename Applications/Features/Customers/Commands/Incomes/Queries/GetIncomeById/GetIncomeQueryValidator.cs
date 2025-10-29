using Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeById;
using Applications.Features.Customers.Commands.Incomes.Queries.GetIncomeById;
using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.Queries.GetIncomeByCustomerId
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
