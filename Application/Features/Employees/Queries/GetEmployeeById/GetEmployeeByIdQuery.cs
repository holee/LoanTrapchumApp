using Application.Utils;
using Domains.Models.Lists;
using ErrorOr;

namespace Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(string EmployeeId, int BranchId) : IRequest<ErrorOr<EntityNames>>;
