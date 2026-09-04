using Application.Responses.ViewModels.Employees;
using ErrorOr;

namespace Application.Features.Employees.Commands.ChangeEmployeeState;

public sealed record ChangeEmployeeStateCommand(string? EmployeeId, bool State) : IRequest<ErrorOr<VEmployeeList>>;
