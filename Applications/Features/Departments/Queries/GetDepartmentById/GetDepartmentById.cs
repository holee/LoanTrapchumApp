using ErrorOr;

namespace Applications.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentById(int? DepartmentId, int? BranchId) : IRequest<ErrorOr<Department>>;
