using ErrorOr;

namespace Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentById(int? DepartmentId, int? BranchId) : IRequest<ErrorOr<Department>>;
