using ErrorOr;

namespace Application.Features.Vendors.Commands.ChangeVendorStatus;

public record ChangeVendorStatusCommand(string? VendorId, bool IsActive) : IRequest<ErrorOr<Updated>>;
