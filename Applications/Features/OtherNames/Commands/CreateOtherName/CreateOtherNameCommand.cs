using Applications.Features.OtherNames.Dto;
using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.CreateOtherName;

public record CreateOtherNameCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VOtherNames>>;
