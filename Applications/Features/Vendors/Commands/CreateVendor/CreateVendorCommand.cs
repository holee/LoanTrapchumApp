using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Requests;
using Applications.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Applications.Features.Vendors.Commands.CreateVendor;

public record CreateVendorCommand(EntityVendorCreate Dto) : IRequest<ErrorOr<VendorDto>>;
