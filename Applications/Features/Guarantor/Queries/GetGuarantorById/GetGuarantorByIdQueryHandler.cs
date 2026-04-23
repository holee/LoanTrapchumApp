using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Guarantor.Queries.GetGuarantorById;

public sealed class GetGuarantorByIdQueryHandler(IGuarantorService guarantorService)
    : IRequestHandler<GetGuarantorByIdQuery, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(GetGuarantorByIdQuery request, CancellationToken cancellationToken)
    {
        return await guarantorService.GetGuarantorByIdAsync(request.GuarantorId, request.AllGuarantors, request.IncludeRequest);
    }
}
