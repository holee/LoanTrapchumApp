using Application.Commons.Mappers;
using Application.Interfaces.Lists;
using Application.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.UpdateOtherName;

public sealed class UpdateOtherNameCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<UpdateOtherNameCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(UpdateOtherNameCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.MapNamesEdit();
        return await otherNameService.UpdateAsync(entity, cancellationToken);
    }
}
