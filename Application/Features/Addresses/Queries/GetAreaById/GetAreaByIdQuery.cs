using Application.Features.Addresses.Dto;
using ErrorOr;

namespace Application.Features.Addresses.Queries.GetAreaById;

public record GetAreaByIdQuery(long? Id) : IRequest<ErrorOr<AreaDto>>;
