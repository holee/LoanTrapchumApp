using ErrorOr;

namespace Applications.Features.Identity.Command.ToggleUserActive;

public record ToggleUserActiveCommand(string UserId) : IRequest<ErrorOr<Updated>>;
