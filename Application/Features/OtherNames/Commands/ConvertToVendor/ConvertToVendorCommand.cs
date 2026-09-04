using Application.Features.OtherNames.Dto;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.ConvertToVendor;

public record ConvertToVendorCommand(string? Id, ConvertCreateVendor Dto) : IRequest<ErrorOr<bool>>;
