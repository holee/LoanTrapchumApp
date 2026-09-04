using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.DeleteOtherName;

public sealed class DeleteOtherNameCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<DeleteOtherNameCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteOtherNameCommand request, CancellationToken cancellationToken)
    {
        return await otherNameService.DeleteAsync(request.Id, cancellationToken);
    }
}
