using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchById;

public record GetBranchByIdQuery(int Id) : IRequest<ErrorOr<Branch>>;
