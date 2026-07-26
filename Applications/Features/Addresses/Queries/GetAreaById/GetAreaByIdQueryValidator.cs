namespace Applications.Features.Addresses.Queries.GetAreaById;

public sealed class GetAreaByIdQueryValidator : AbstractValidator<GetAreaByIdQuery>
{
    public GetAreaByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThan(0);
    }
}
