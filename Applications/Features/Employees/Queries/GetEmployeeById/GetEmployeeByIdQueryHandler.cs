using Applications.Interfaces.Employees;
using Applications.Utils;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdQueryHandler(IEmployeeService empService) : IRequestHandler<GetEmployeeByIdQuery, ErrorOr<EntityNames>>
{
    public async Task<ErrorOr<EntityNames>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await empService.GetByIdAsync(request.EmployeeId, request.BranchId);
    }
}