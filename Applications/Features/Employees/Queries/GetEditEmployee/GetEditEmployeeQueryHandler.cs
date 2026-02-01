using Applications.Commons.Mappers;
using Applications.Features.Employees.Dto;
using Applications.Interfaces.Employees;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEditEmployee;

public sealed class GetEditEmployeeQueryHandler(IEmployeeService employeeService) : IRequestHandler<GetEditEmployeeQuery, ErrorOr<EntityEmployeeCreate>>
{
    public async Task<ErrorOr<EntityEmployeeCreate>> Handle(GetEditEmployeeQuery request, CancellationToken cancellationToken = default)
    {
        var emp = await employeeService.GetByIdAsync(request.EmployeeId, null, cancellationToken);

        if (emp.IsError)
        {
            return Error.NotFound("No Emploee found");
        }

        return emp.Value.ToEmployeeCreate();

    }
}