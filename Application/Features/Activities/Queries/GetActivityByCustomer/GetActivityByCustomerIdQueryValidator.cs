using FluentValidation;
namespace Application.Features.Activities.ActitvityQuery.GetActivityByCustomer;

public class GetActivityByCustomerIdQueryValidator : AbstractValidator<GetActivityByCustomerIdQuery>
{
    public GetActivityByCustomerIdQueryValidator()
    {
        RuleFor(x=>x.CustomerId).NotEmpty()
                                    .WithMessage("CustomerId is required");
    }
}