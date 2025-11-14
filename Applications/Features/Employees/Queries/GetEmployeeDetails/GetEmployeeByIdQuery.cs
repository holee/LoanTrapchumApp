using Applications.Utils;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeDetailsQuery(string EmployeeId, int BranchId) : IRequest<ErrorOr<EntityNames>>;
 