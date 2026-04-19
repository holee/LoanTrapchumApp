using Applications.Features.Vendors.Dto.Requests;
using Applications.Responses.ViewModels.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Commands.UpdateVendor;

public record UpdateVendorCommand(VendorDtoUpdate Dto) : IRequest<ErrorOr<VendorVm>>;
