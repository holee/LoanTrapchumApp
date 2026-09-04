using Application.Features.Addresses.Dto;
using ErrorOr;

namespace Application.Features.Addresses.Command.CreateArea;

public record CreateAreaCommand(AreaDtoCreate Area) : IRequest<ErrorOr<AreaDto?>>;
