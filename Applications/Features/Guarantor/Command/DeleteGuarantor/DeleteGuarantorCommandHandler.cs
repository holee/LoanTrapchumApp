using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Guarantor.Command.DeleteGuarantor;

public sealed class DeleteGuarantorCommandHandler(IGuarantorService guarantorService)
    : IRequestHandler<DeleteGuarantorCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteGuarantorCommand request, CancellationToken cancellationToken)
    {
        return await guarantorService.DeleteAsync(request.GuarantorId, cancellationToken);
    }
}
