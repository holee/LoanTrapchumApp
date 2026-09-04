using Application.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Application.Features.AdditionalInformations.Command.CreateSalutation;

public record CreateSalutationCommand(SalutationDtoCreate Request) : IRequest<ErrorOr<SalutationDto>>;