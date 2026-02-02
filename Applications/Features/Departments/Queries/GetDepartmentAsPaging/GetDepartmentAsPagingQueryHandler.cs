using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;

namespace Applications.Features.Departments.Queries.GetDepartmentAsPaging;

public sealed class GetDepartmentAsPagingQueryHandler(IDepartmentService departmentService) : IRequestHandler<GetDepartmentAsPagingQuery, Pager<VDepartment>>
{
    public Task<Pager<VDepartment>> Handle(GetDepartmentAsPagingQuery request, CancellationToken cancellationToken = default)
    {
        return departmentService.GetDepartments(request.DepartmentId, request.Param, cancellationToken);
    }
}