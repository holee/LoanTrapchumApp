using Applications.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.CreateOccupation;

public record CreateOccupationCommand(OccupationDtoCreate Request) : IRequest<ErrorOr<OccupationDto>>;
