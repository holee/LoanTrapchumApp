using Applications.Features.Addresses.Dto;
using ErrorOr;

namespace Applications.Features.Addresses.Command.UpdateArea;

public record UpdateAreaCommand(AreaDtoUpdate Area) : IRequest<ErrorOr<AreaDto?>>;
