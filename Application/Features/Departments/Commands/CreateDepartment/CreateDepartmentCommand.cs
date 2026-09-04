using Application.Features.Departments.Dto;
using ErrorOr;

namespace Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(DepartmentDtoCreate Request) : IRequest<ErrorOr<VDepartment>>;
 