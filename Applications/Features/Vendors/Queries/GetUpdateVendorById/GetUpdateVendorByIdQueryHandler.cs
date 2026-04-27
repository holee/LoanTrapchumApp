using ActualLab;
using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using ErrorOr;
using Applications.Commons.Mappers;
namespace Applications.Features.Vendors.Queries.GetUpdateVendorById;

public sealed class GetUpdateVendorByIdQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetUpdateVendorByIdQuery, ErrorOr<EntityVendorCreate>>
{
    public async Task<ErrorOr<EntityVendorCreate>> Handle(GetUpdateVendorByIdQuery request, CancellationToken cancellationToken)
    {

        return await vendorService.GetEditVendorByIdAsync(request.VendorId, cancellationToken);

        //if(result.IsError)
        //{
        //    return result.Errors;
        //}

        //var model = result.Value.ToEntityVendorCreate();

        //return model;
    }
}
