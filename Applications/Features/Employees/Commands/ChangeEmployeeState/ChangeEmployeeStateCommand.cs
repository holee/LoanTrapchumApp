using Applications.Responses.ViewModels.Employees;
using ErrorOr;

namespace Applications.Features.Employees.Commands.ChangeEmployeeState;

public sealed record ChangeEmployeeStateCommand(string? EmployeeId, bool State) : IRequest<ErrorOr<VEmployeeList>>;
