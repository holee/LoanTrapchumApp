using Application.Features.Guarantor.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;
using Mapster;

namespace Application.Features.OtherNames.Commands.CreateGuarantor;

public sealed class CreateOtherNameGuarantorCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<CreateOtherNameGuarantorCommand, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(CreateOtherNameGuarantorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<Domains.Models.Lists.EntityNames>();
        return await otherNameService.CreateGaurantorAsync(entity, cancellationToken);
    }
}
