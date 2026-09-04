using Application.Features.Addresses.Dto;
using ErrorOr;

namespace Application.Features.Addresses.Command.UpdateArea;

public record UpdateAreaCommand(AreaDtoUpdate Area) : IRequest<ErrorOr<AreaDto?>>;
