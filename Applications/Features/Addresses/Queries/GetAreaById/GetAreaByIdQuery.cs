using Applications.Features.Addresses.Dto;
using ErrorOr;

namespace Applications.Features.Addresses.Queries.GetAreaById;

public record GetAreaByIdQuery(long? Id) : IRequest<ErrorOr<AreaDto>>;
