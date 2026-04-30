using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Features.OtherNames.Queries.GetOtherNameById;

public record GetOtherNameByIdQuery(string? Id) : IRequest<ErrorOr<EntityNames>>;
