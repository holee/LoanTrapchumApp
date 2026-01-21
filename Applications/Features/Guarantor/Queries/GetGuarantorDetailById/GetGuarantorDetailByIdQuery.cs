using Applications.Features.Guarantor.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var rsp = await _guarantorRepository.GetGuarantorWithByIdAsync(request.Id);

        if (rsp is { IsSuccess: false, Data: null })
        {
            return Error.NotFound("NotFound", $"Guarantor with Id {request.Id} not found.");
        }
        return rsp.Data!;
    }
}