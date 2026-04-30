using Applications.Features.OtherNames.Dto;
using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.UpdateOtherName;

public record UpdateOtherNameCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VOtherNames>>;
