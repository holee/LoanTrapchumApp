using Applications.Features.Guarantor.Dtos;
using ErrorOr;

namespace Applications.Features.Guarantor.Queries.GetGuarantorDetailById;

public record GetGuarantorDetailByIdQuery(string? Id) : IRequest<ErrorOr<VGuarantor>>
{

}
