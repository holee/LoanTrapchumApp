namespace Applications.Features.Identity.Queries.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage("User Id is required.")
            .NotEmpty()
                .WithMessage("User Id is required.");
    }
}
