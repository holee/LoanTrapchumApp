namespace Applications.Features.Identity.Queries.GetUserDetailById;

public sealed class GetUserDetailsByIdQueryValidator : AbstractValidator<GetUserDetailsByIdQuery>
{
    public GetUserDetailsByIdQueryValidator()
    {
        RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("User Id cannot be null.")
                    .NotEmpty().WithMessage("User Id is required.");
    }
}