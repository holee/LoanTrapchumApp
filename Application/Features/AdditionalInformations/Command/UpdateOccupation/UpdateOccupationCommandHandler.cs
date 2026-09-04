using Application.Commons.Mappers;
using Application.Features.AdditionalInformations.Dto;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.AdditionalInformations.Command.UpdateOccupation;

public sealed class UpdateOccupationCommandHandler(IOccupationService occupation) : IRequestHandler<UpdateOccupationCommand, ErrorOr<OccupationDto>>
{
    public async Task<ErrorOr<OccupationDto>> Handle(UpdateOccupationCommand request, CancellationToken cancellationToken = default)
    {
        return await occupation.UpdateAsync(request.Request, cancellationToken);
    }
}
