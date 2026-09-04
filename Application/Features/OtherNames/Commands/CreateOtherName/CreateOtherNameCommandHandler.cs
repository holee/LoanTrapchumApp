using Application.Commons.Mappers;
using Application.Interfaces.Lists;
using Application.Responses.ViewModels.OtherNames;
using ErrorOr;
using Mapster;

namespace Application.Features.OtherNames.Commands.CreateOtherName;

public sealed class CreateOtherNameCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<CreateOtherNameCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(CreateOtherNameCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<Domains.Models.Lists.EntityNames>();
        return await otherNameService.CreateAsync(entity, cancellationToken);
    }
}
