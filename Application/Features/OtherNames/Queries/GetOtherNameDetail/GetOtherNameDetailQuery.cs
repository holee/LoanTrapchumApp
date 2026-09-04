using ErrorOr;

namespace Application.Features.OtherNames.Queries.GetOtherNameDetail;

public record GetOtherNameDetailQuery(string? Id) : IRequest<ErrorOr<VOtherNames>>;
