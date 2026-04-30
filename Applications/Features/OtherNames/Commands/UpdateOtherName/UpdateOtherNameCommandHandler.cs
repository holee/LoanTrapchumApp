using Applications.Commons.Mappers;
using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.UpdateOtherName;

public sealed class UpdateOtherNameCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<UpdateOtherNameCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(UpdateOtherNameCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.MapNamesEdit();
        return await otherNameService.UpdateAsync(entity, cancellationToken);
    }
}
