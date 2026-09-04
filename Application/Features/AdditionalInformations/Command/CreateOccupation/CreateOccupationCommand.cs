using Application.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Application.Features.AdditionalInformations.Command.CreateOccupation;

public record CreateOccupationCommand(OccupationDtoCreate Request) : IRequest<ErrorOr<OccupationDto>>;
