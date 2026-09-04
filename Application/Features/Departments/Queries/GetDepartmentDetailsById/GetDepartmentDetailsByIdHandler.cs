using Application.Features.Departments.Dto;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentDetailsByIdHandler(IDepartmentService departmentService) : IRequestHandler<GetDepartmentDetailsById, ErrorOr<VDepartment>>
{

    public async Task<ErrorOr<VDepartment>> Handle(GetDepartmentDetailsById request, CancellationToken cancellationToken)
    {
        return await departmentService.GetDepartmentDetailsById(request.DepartmentId, request.BranchId, cancellationToken);
    }
}