using Applications.Interfaces.Employees;
using Applications.Responses.ViewModels.Employees;
using ErrorOr;

namespace Applications.Features.Employees.Commands.ChangeEmployeeState;

public sealed class ChangeEmployeeStateCommandHandler(IEmployeeService employeeService) : IRequestHandler<ChangeEmployeeStateCommand, ErrorOr<VEmployeeList>>
{
    public async Task<ErrorOr<VEmployeeList>> Handle(ChangeEmployeeStateCommand request, CancellationToken cancellationToken)
    {
        return await employeeService.UpdateEmployeeStateAsync(request.EmployeeId, request.State, cancellationToken);
    }
}