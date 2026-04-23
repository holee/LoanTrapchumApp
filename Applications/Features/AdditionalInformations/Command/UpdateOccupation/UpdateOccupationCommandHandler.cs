using Applications.Commons.Mappers;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.UpdateOccupation;

public sealed class UpdateOccupationCommandHandler(IOccupationService occupation) : IRequestHandler<UpdateOccupationCommand, ErrorOr<OccupationDto>>
{
    public async Task<ErrorOr<OccupationDto>> Handle(UpdateOccupationCommand request, CancellationToken cancellationToken = default)
    {
        return await occupation.UpdateAsync(request.Request, cancellationToken);
    }
}
