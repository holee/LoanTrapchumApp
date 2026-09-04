using Application.Features.OtherNames.Dto;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.ConvertToEmployee;

public record ConvertToEmployeeCommand(string? Id, ConvertCreateEmployee Dto) : IRequest<ErrorOr<bool>>;
