using Application.Features.OtherNames.Dto;
using Application.Features.Vendors.Dto.Requests;
using Application.Features.Vendors.Dto.Responses;
using ErrorOr;

namespace Application.Features.Vendors.Commands.CreateVendor;

public record CreateVendorCommand(EntityVendorCreate Dto) : IRequest<ErrorOr<VendorDto>>;
