using Applications.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.UpdateOccupation;

public record UpdateOccupationCommand(OccupationDtoUpate Request) : IRequest<ErrorOr<OccupationDto>>;
