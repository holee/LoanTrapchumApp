using Applications.Features.Guarantor.Dtos;
using ErrorOr;

namespace Applications.Features.Guarantor.Queries.GetGuarantorById;

public record GetGuarantorByIdQuery(string? GuarantorId, bool AllGuarantors = false, bool IncludeRequest = true)
    : IRequest<ErrorOr<VGuarantor>>;
