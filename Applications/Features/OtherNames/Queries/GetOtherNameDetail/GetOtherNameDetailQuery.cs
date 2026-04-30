using ErrorOr;

namespace Applications.Features.OtherNames.Queries.GetOtherNameDetail;

public record GetOtherNameDetailQuery<T>(string? Id) : IRequest<ErrorOr<T>> where T : class;
