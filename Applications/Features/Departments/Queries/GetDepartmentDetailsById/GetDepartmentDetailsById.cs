using Applications.Features.Departments.Dto;
using ErrorOr;

namespace Applications.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentDetailsById(int? DepartmentId, int? BranchId) : IRequest<ErrorOr<VDepartment>>; 
 