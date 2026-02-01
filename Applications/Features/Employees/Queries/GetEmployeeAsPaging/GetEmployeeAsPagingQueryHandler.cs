using Applications.Interfaces.Employees;
using Applications.Responses.ViewModels.Employees;

namespace Applications.Features.Employees.Queries.GetEmployeeAsPaging;

public sealed class GetEmployeeAsPagingQueryHandler(IEmployeeService employeeService) : IRequestHandler<GetEmployeeAsPagingQuery, Pager<VEmployeeList>>
{
    public async Task<Pager<VEmployeeList>> Handle(GetEmployeeAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await employeeService.GetEmployeesAsync(request.BranchId, request.EmployeeId, request.param);
    }
}
