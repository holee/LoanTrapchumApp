using Application.Utils;
using ErrorOr;
namespace Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(string? EmployeeId,int? BranchId) : IRequest<ErrorOr<DeleteResult>>;
