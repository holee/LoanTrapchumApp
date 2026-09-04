using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchById;

public record GetBranchByIdQuery(int Id) : IRequest<ErrorOr<Branch>>;
