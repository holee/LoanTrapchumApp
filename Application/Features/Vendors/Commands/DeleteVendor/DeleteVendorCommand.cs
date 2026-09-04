using ErrorOr;

namespace Application.Features.Vendors.Commands.DeleteVendor;

public record DeleteVendorCommand(string? VendorId) : IRequest<ErrorOr<Deleted>>;
