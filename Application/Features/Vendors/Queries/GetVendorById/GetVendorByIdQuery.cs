using Application.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Application.Features.Vendors.Queries.GetVendorById;

public record GetVendorByIdQuery(string? VendorId) : IRequest<ErrorOr<VendorDto>>;
