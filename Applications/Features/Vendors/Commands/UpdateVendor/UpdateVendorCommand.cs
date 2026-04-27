using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Requests;
using Applications.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Applications.Features.Vendors.Commands.UpdateVendor;

public record UpdateVendorCommand(EntityVendorCreate Dto) : IRequest<ErrorOr<VendorDto>>;
