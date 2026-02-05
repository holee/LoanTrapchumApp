using Applications.Features.Departments.Dto;
using ErrorOr;

namespace Applications.Features.Departments.Commands.ChangeDepartmentState;

public record ChangeDepartmentStateCommand(int? DepartmentId, bool IsActive) : IRequest<ErrorOr<VDepartment>>;
