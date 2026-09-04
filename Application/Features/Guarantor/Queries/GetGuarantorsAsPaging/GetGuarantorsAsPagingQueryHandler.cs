using Application.Interfaces.Lists;

namespace Application.Features.Guarantor.Queries.GetGuarantorsAsPaging;

public sealed class GetGuarantorsAsPagingQueryHandler(IGuarantorService guarantorService)
    : IRequestHandler<GetGuarantorsAsPagingQuery, Pager<VGuarantor>>
{
    public async Task<Pager<VGuarantor>> Handle(GetGuarantorsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await guarantorService.GetGuarantorsAsync(request.Id, request.Param, cancellationToken);
    }
}
