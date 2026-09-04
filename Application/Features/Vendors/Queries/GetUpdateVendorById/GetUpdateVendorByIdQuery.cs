using Application.Features.OtherNames.Dto;
using ErrorOr;

namespace Application.Features.Vendors.Queries.GetUpdateVendorById;

public record GetUpdateVendorByIdQuery(string? VendorId) : IRequest<ErrorOr<EntityVendorCreate>>; 
