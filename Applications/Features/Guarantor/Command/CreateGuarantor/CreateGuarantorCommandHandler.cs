using Applications.Commons.Mappers;
using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Guarantor.Command.CreateGuarantor;

public sealed class CreateGuarantorCommandHandler(IGuarantorService _guarantorService) : IRequestHandler<CreateGuarantorCommand, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(CreateGuarantorCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.MapGuarantorCreate();

        var result = await _guarantorService.CreateGuarantorAsync(model);

        if (result is null)
        {
            return Error.Failure("Failed to create guarantor.");
        }

        return result;
    }
}