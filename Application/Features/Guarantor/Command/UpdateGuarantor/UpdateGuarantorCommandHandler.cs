using Application.Commons.Mappers;
using Application.Features.Guarantor.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Guarantor.Command.UpdateGuarantor;

public sealed class UpdateGuarantorCommandHandler(IGuarantorService guarantorService)
    : IRequestHandler<UpdateGuarantorCommand, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(UpdateGuarantorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.MapGuarantorEdit();

        var updated = await guarantorService.UpdateAsync(entity, cancellationToken);

        if (updated.IsError)
        {
            return updated.Errors;
        }

        return await guarantorService.GetGuarantorByIdAsync(entity.Id, cancellationToken);
    }
}
