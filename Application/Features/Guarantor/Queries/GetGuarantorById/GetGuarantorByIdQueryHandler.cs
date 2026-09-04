using Application.Features.Guarantor.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Guarantor.Queries.GetGuarantorById;

public sealed class GetGuarantorByIdQueryHandler(IGuarantorService guarantorService) : IRequestHandler<GetGuarantorByIdQuery, ErrorOr<VGuarantor>>
{
    public async Task<ErrorOr<VGuarantor>> Handle(GetGuarantorByIdQuery request, CancellationToken cancellationToken)
    {
        return await guarantorService.GetGuarantorByIdAsync(request.GuarantorId, cancellationToken);
    }
}
