using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;
using Mapster;

namespace Applications.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(IDepartmentService departmentService) : IRequestHandler<CreateDepartmentCommand, ErrorOr<VDepartment>>
{
    public async Task<ErrorOr<VDepartment>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {

        var department = request.Request.Adapt<Department>();

        return await departmentService.CreateAsync(department, cancellationToken);
    }
}
