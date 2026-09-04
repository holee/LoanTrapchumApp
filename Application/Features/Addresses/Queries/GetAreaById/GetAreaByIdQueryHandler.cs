using Application.Interfaces.Addresses;
using ErrorOr;

namespace Application.Features.Addresses.Queries.GetAreaById;

public sealed class GetAreaByIdQueryHandler(IAddressService addressService) : IRequestHandler<GetAreaByIdQuery, ErrorOr<AreaDto>>
{
    public async Task<ErrorOr<AreaDto>> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await addressService.GetAreaAsync(request.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the area with ID {request.Id}: {ex.Message}", ex);
        }

    }
}
