using Applications.Commons.Mappers;
using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;
using Mapster;

namespace Applications.Features.OtherNames.Commands.CreateOtherName;

public sealed class CreateOtherNameCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<CreateOtherNameCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(CreateOtherNameCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<Domains.Models.Lists.EntityNames>();
        return await otherNameService.CreateAsync(entity, cancellationToken);
    }
}
