using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(string? EmployeeId,int? BranchId) : IRequest<ErrorOr<DeleteResult>>;
