using Applications.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Applications.Features.Vendors.Queries.GetVendorById;

public record GetVendorByIdQuery(string? VendorId) : IRequest<ErrorOr<VendorDto>>;
