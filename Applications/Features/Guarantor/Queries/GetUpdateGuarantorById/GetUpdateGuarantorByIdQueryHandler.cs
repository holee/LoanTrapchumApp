using Applications.Interfaces.Lists;
using ErrorOr;
using Applications.Commons.Mappers;

namespace Applications.Features.Guarantor.Queries.GetUpdateGuarantorById;

public sealed class GetUpdateGuarantorByIdQueryHandler(IGuarantorService guarantorService)
    : IRequestHandler<GetUpdateGuarantorByIdQuery, ErrorOr<EntityGuarantorCreate>>
{
    public async Task<ErrorOr<EntityGuarantorCreate>> Handle(GetUpdateGuarantorByIdQuery request, CancellationToken cancellationToken)
    {
        var rsp = await guarantorService.GetByIdAsync(request.GuarantorId);

        if (rsp.IsError)
        {
            return rsp.Errors;
        }

        var guarantor = rsp.Value.MapGuarantor();

        return guarantor;
    }
}