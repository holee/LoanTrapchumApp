using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;
using Mapster;

namespace Applications.Features.Departments.Commands.EditDepartment;

public class EditDepartmentCommandHandler(IDepartmentService departmentService) : IRequestHandler<EditDepartmentCommand, ErrorOr<VDepartment>>
{
    public async Task<ErrorOr<VDepartment>> Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
    {

        var department = request.Request.Adapt<Department>();

        return await departmentService.UpdateAsync(department, cancellationToken);
    }
}
