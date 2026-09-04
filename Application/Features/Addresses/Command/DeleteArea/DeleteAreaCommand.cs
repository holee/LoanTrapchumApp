using ErrorOr;

namespace Application.Features.Addresses.Command.DeleteArea;

public record DeleteAreaCommand(long? Id) : IRequest<ErrorOr<bool>>;
