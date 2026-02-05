using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Departments.Commands.ChangeDepartmentState;

public sealed class ChangeDepartmentStateCommandHandler(IDepartmentService departmentService) : IRequestHandler<ChangeDepartmentStateCommand, ErrorOr<VDepartment>>
{
    public Task<ErrorOr<VDepartment>> Handle(ChangeDepartmentStateCommand request, CancellationToken cancellationToken = default)
    {
        return departmentService.ActivateOrDeactivate(request.DepartmentId, request.IsActive, cancellationToken);
    }
}