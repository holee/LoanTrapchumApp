using ErrorOr;

namespace Application.Features.Identity.Command.ToggleUserActive;

public record ToggleUserActiveCommand(string UserId) : IRequest<ErrorOr<Updated>>;
