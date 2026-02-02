using Applications.Features.Departments.Dto;
using ErrorOr;

namespace Applications.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(DepartmentDtoCreate Request) : IRequest<ErrorOr<VDepartment>>;
 