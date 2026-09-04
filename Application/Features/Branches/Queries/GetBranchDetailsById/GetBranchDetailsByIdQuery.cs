using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchDetailsById;

public record GetBranchDetailsByIdQuery(int? Id, params string[] columns) : IRequest<ErrorOr<VBranch?>>;
