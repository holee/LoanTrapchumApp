using Applications.Features.Branches.Dtos;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Branches.Queries.GetBranchByUser;

public record GetBranchByUserQuery(string? UserId, int? BranchId) : IRequest<ErrorOr<IEnumerable<VBranch>>>
{
}
