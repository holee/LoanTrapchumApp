using Application.Features.Guarantor.Dtos;
using ErrorOr;

namespace Application.Features.Guarantor.Queries.GetGuarantorDetailById;

public record GetGuarantorDetailByIdQuery(string? Id) : IRequest<ErrorOr<VGuarantor>>
{

}
