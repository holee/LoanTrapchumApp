using Application.Features.OtherNames.Dto;
using Application.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.UpdateOtherName;

public record UpdateOtherNameCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VOtherNames>>;
