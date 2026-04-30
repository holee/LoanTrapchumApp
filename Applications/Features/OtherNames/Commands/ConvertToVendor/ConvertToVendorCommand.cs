using Applications.Features.OtherNames.Dto;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.ConvertToVendor;

public record ConvertToVendorCommand(string? Id, ConvertCreateVendor Dto) : IRequest<ErrorOr<bool>>;
