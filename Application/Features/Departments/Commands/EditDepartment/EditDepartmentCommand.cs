using Application.Features.Departments.Dto;
using ErrorOr;

namespace Application.Features.Departments.Commands.EditDepartment;

public record EditDepartmentCommand(DepartmentDtoUpdate Request) : IRequest<ErrorOr<VDepartment>>;  
