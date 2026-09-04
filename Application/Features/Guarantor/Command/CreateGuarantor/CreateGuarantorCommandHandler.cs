using Application.Commons.Mappers;
using Application.Features.Guarantor.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Guarantor.Command.CreateGuarantor;

public sealed class CreateGuarantorCommandHandler(IGuarantorService _guarantorService) : IRequestHandler<CreateGuarantorCommand, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(CreateGuarantorCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.MapGuarantorCreate();

        var result = await _guarantorService.CreateAsync(model);

        return result;
    }
}