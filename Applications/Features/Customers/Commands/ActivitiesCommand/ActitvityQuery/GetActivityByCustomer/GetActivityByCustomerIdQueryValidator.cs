using Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityByCustomer;
using FluentValidation;
namespace Applications.Features.Customers.Commands.ActivitiesCommand.ActitvityQuery.GetActivityByCustomer;

public class GetActivityByCustomerIdQueryValidator : AbstractValidator<GetActivityByCustomerIdQuery>
{
    public GetActivityByCustomerIdQueryValidator()
    {
        RuleFor(x=>x.CustomerId).NotEmpty()
                                    .WithMessage("CustomerId is required");
    }
}