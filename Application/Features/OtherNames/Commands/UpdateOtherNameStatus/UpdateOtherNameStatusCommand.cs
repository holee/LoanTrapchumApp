using Application.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.UpdateOtherNameStatus;

public record UpdateOtherNameStatusCommand(string? Id, bool State) : IRequest<ErrorOr<VOtherNames>>;
