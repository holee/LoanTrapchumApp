using Applications.Features.Departments.Dto;
using ErrorOr;

namespace Applications.Features.Departments.Commands.EditDepartment;

public record EditDepartmentCommand(DepartmentDtoUpdate Request) : IRequest<ErrorOr<VDepartment>>;  
