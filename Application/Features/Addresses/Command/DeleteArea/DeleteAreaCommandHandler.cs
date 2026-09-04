using Application.Interfaces.Addresses;
using ErrorOr;

namespace Application.Features.Addresses.Command.DeleteArea;

public sealed class DeleteAreaCommandHandler(IAddressService addressService) : IRequestHandler<DeleteAreaCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteAreaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await addressService.DeleteAreasAsync(request.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the area with ID {request.Id}.", ex);
        }

    }
}
