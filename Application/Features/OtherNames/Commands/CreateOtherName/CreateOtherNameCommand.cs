using Application.Features.OtherNames.Dto;
using Application.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.CreateOtherName;

public record CreateOtherNameCommand(EntityNamesCreate Dto) : IRequest<ErrorOr<VOtherNames>>;
