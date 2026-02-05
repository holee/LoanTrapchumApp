using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchDetailsById;

public record GetBranchDetailsByIdQuery(int? Id, params string[] columns) : IRequest<ErrorOr<VBranch?>>;
