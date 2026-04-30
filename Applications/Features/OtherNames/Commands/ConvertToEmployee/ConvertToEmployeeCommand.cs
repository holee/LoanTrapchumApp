using Applications.Features.OtherNames.Dto;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.ConvertToEmployee;

public record ConvertToEmployeeCommand(string? Id, ConvertCreateEmployee Dto) : IRequest<ErrorOr<bool>>;
