namespace Applications.Features.OtherNames.Queries.GetOtherNameById;

public sealed class GetOtherNameByIdQueryValidator : AbstractValidator<GetOtherNameByIdQuery>
{
    public GetOtherNameByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
