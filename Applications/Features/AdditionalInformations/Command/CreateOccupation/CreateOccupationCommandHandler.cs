using Applications.Commons.Mappers;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Command.CreateOccupation;

public sealed class CreateOccupationCommandHandler(IOccupationService occupation) : IRequestHandler<CreateOccupationCommand, ErrorOr<OccupationDto>>
{
    public Task<ErrorOr<OccupationDto>> Handle(CreateOccupationCommand request, CancellationToken cancellationToken = default)
    {

        var model = request.Request.MapCreate();

        return occupation.CreateAsync(model);
    }
}
