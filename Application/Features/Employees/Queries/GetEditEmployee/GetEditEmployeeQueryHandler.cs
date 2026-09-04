using Application.Commons.Mappers;
using Application.Features.Employees.Dto;
using Application.Interfaces.Employees;
using ErrorOr;

namespace Application.Features.Employees.Queries.GetEditEmployee;

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