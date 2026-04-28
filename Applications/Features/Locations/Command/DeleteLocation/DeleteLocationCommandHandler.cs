using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Locations.Command.DeleteLocation;

public sealed class DeleteLocationCommandHandler(ILocationService locationService)
    : IRequestHandler<DeleteLocationCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        return await locationService.DeleteAsync(request.Id, cancellationToken);
    }
}
