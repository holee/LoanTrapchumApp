using ActualLab;
using Application.Features.OtherNames.Dto;
using Application.Features.Vendors.Dto.Responses;
using Application.Interfaces.Vendors;
using ErrorOr;
using Application.Commons.Mappers;
namespace Application.Features.Vendors.Queries.GetUpdateVendorById;

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
