using ErrorOr;

namespace Applications.Features.Locations.Command.DeleteLocation;

public record DeleteLocationCommand(long Id) : IRequest<ErrorOr<string>>;
