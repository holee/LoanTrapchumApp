using Application.Features.Departments.Dto;
using ErrorOr;

namespace Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentDetailsById(int? DepartmentId, int? BranchId) : IRequest<ErrorOr<VDepartment>>; 
 