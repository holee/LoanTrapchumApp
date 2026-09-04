using Application.Features.Guarantor.Dtos;
using ErrorOr;

namespace Application.Features.Guarantor.Queries.GetGuarantorById;

public record GetGuarantorByIdQuery(string? GuarantorId, bool AllGuarantors = false, bool IncludeRequest = true)
    : IRequest<ErrorOr<VGuarantor>>;
