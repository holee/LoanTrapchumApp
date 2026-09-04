using Application.Features.Departments.Dto;
using ErrorOr;

namespace Application.Features.Departments.Commands.ChangeDepartmentState;

public record ChangeDepartmentStateCommand(int? DepartmentId, bool IsActive) : IRequest<ErrorOr<VDepartment>>;
