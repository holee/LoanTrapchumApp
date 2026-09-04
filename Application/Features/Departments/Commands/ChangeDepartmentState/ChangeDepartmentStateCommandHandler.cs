using Application.Features.Departments.Dto;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Departments.Commands.ChangeDepartmentState;

public sealed class ChangeDepartmentStateCommandHandler(IDepartmentService departmentService) : IRequestHandler<ChangeDepartmentStateCommand, ErrorOr<VDepartment>>
{
    public Task<ErrorOr<VDepartment>> Handle(ChangeDepartmentStateCommand request, CancellationToken cancellationToken = default)
    {
        return departmentService.ActivateOrDeactivate(request.DepartmentId, request.IsActive, cancellationToken);
    }
}