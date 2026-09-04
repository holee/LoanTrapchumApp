using FluentValidation;

namespace Application.Features.Activities.ActitvityQuery.GetActivityById;

public sealed class GetActivityByIdQueryValidator : AbstractValidator<GetActivityByIdQuery>
{
    public GetActivityByIdQueryValidator()
    {
        RuleFor(x=>x.Id).GreaterThan(0)
                .WithMessage("Id must be greater than zero");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage("CustomerId cannot be null or empty.")
            .NotEmpty()
                .WithMessage("CustomerId cannot be null or empty.");
    }
}