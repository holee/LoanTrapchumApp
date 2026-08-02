using ErrorOr;

namespace Applications.Features.OtherNames.Queries.GetOtherNameDetail;

public record GetOtherNameDetailQuery(string? Id) : IRequest<ErrorOr<VOtherNames>>;
