using FluentValidation;

namespace Application.Features.Activities.ActitvityQuery.GetActivitiesQuery;

public sealed class GetCustomerActivityQueryValidator : AbstractValidator<GetCustomerActivityQuery>
{
    public GetCustomerActivityQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty()
                                  .WithMessage("Customer ID is required.");
    }

}