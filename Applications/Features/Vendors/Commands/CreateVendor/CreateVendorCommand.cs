using Applications.Features.Vendors.Dto.Requests;
using Applications.Responses.ViewModels.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Commands.CreateVendor;

public record CreateVendorCommand(VendorDtoCreate Dto) : IRequest<ErrorOr<VendorVm>>;
