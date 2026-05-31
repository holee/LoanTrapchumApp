namespace Applications.Features.Identity.Queries.GetUserPermissions;

public sealed class GetUserPersionsQueryValidator : AbstractValidator<GetUserPermissionsQuery>
{
    public GetUserPersionsQueryValidator()
    {
        RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage("User Id is required.")
            .NotEmpty()
                .WithMessage("User Id is required.");
    }
}
