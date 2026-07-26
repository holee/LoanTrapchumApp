using ErrorOr;

namespace Applications.Features.Addresses.Command.DeleteArea;

public record DeleteAreaCommand(long? Id) : IRequest<ErrorOr<bool>>;
