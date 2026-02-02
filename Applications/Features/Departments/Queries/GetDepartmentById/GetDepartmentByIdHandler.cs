using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdHandler(IDepartmentService departmentService) : IRequestHandler<GetDepartmentById, ErrorOr<Department>>
{
     
    public async Task<ErrorOr<Department>> Handle(GetDepartmentById request, CancellationToken cancellationToken)
    {
        return await departmentService.GetDepartmentById(request.DepartmentId, request.BranchId, cancellationToken);
    }
}