using Applications.Responses.ViewModels.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Queries.GetVendorById;

public record GetVendorByIdQuery(string? VendorId) : IRequest<ErrorOr<VendorVm>>;
