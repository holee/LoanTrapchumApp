using Applications.Interfaces.Employees;
using Applications.Responses.ViewModels.Employees;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeDetailsByIdQueryHandler(IEmployeeService _employeeService) : IRequestHandler<GetEmployeeDetailsByIdQuery, ErrorOr<VEmployeeList>>
{
    public async Task<ErrorOr<VEmployeeList>> Handle(GetEmployeeDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        return await _employeeService.GetEmployeeDetailsByIdAsync(request.EmployeeId, request.BranchId, request.Include);
    }
}
