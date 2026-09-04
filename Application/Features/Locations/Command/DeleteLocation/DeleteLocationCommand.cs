using ErrorOr;

namespace Application.Features.Locations.Command.DeleteLocation;

public record DeleteLocationCommand(long Id) : IRequest<ErrorOr<string>>;
