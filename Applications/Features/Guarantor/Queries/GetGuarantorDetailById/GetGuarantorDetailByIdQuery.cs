using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Guarantor.Queries.GetGuarantorDetailById;

public record GetGuarantorDetailByIdQuery(string? Id) : IRequest<ErrorOr<VGuarantor>>
{

}


public sealed class GetGuarantorDetailByIdQueryHandler : IRequestHandler<GetGuarantorDetailByIdQuery, ErrorOr<VGuarantor>>
{
    private readonly IGuarantorService _guarantorRepository;
    public GetGuarantorDetailByIdQueryHandler(IGuarantorService guarantorRepository)
    {
        _guarantorRepository = guarantorRepository;
    }
    public async Task<ErrorOr<VGuarantor>> Handle(GetGuarantorDetailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _guarantorRepository.GetGuarantorWithByIdAsync(request.Id);
    }
}