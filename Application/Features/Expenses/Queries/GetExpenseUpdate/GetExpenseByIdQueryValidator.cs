using FluentValidation;

namespace Application.Features.Expenses.Queries.GetExpenseById
{
    public sealed class GetExpenseByCustomerQueryValidator : AbstractValidator<GetExpenseByIdUpdateQuery>
    {
        public GetExpenseByCustomerQueryValidator()
        {

            RuleFor(x=>x.Id).GreaterThan(0)
                                   .WithMessage("Please provide valid expense id");

            RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Please provide customerId");
        }
    }
}
