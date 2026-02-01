using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Departments.Queries.GetDepartmentAsPaging;

public record GetDepartmentAsPagingQuery(int? DepartmentId, QueryParam Param) : IRequest<Pager<VDepartment>>;

public sealed class GetDepartmentAsPagingQueryHandler(IDepartmentService departmentService) : IRequestHandler<GetDepartmentAsPagingQuery, Pager<VDepartment>>
{
    public Task<Pager<VDepartment>> Handle(GetDepartmentAsPagingQuery request, CancellationToken cancellationToken = default)
    {
        return departmentService.GetDepartments(request.DepartmentId, request.Param, cancellationToken);
    }
}