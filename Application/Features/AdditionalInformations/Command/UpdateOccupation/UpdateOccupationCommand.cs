using Application.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Application.Features.AdditionalInformations.Command.UpdateOccupation;

public record UpdateOccupationCommand(OccupationDtoUpate Request) : IRequest<ErrorOr<OccupationDto>>;
