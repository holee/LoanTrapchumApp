using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.UpdateOtherNameStatus;

public record UpdateOtherNameStatusCommand(string? Id, bool State) : IRequest<ErrorOr<VOtherNames>>;
