using Applications.Features.AdditionalInformations.Dto;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.CreateSalutation;

public record CreateSalutationCommand(SalutationDtoCreate Request) : IRequest<ErrorOr<SalutationDto>>;