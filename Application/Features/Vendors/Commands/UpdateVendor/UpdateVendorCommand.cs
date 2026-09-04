using Application.Features.OtherNames.Dto;
using Application.Features.Vendors.Dto.Requests;
using Application.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Application.Features.Vendors.Commands.UpdateVendor;

public record UpdateVendorCommand(EntityVendorCreate Dto) : IRequest<ErrorOr<VendorDto>>;
