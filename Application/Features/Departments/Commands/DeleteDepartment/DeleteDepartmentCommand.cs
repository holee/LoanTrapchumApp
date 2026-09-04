using ErrorOr;

namespace Application.Features.Departments.Commands.DeleteDepartment; 

public record DeleteDepartmentCommand(int? DepartmentId,int? BranchId) : IRequest<ErrorOr<string>>;  
 