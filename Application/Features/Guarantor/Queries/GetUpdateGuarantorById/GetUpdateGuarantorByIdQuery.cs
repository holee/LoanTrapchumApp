using ErrorOr;

namespace Application.Features.Guarantor.Queries.GetUpdateGuarantorById;

public record GetUpdateGuarantorByIdQuery(string? GuarantorId) : IRequest<ErrorOr<EntityGuarantorCreate>>;

public sealed class GetUpdateGuarantorByIdQueryValidator : AbstractValidator<GetUpdateGuarantorByIdQuery>
{
    public GetUpdateGuarantorByIdQueryValidator()
    {
        RuleFor(x => x.GuarantorId)
                .NotNull()
                    .WithMessage("GuarantorId is required.")
                .NotEmpty()
                    .WithMessage("GuarantorId is required.");
    }
}