using Applications.Features.OtherNames.Dto;
using ErrorOr;

namespace Applications.Features.Vendors.Queries.GetUpdateVendorById;

public record GetUpdateVendorByIdQuery(string? VendorId) : IRequest<ErrorOr<EntityVendorCreate>>; 
